using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Cartomatic.Utils.Number;

namespace MapHive.Server.Core.DataModel
{
    public static partial class ReadFilterExtensions
    {
        /// <summary>
        /// Applies a read filter on an IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <param name="greedy"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyReadFilters<T>(this IQueryable<T> query, IEnumerable<ReadFilter> filters, bool greedy = true)
        {
            var targetType = query.GetType().GetGenericArguments().FirstOrDefault();
            if (targetType == null || filters == null || !filters.Any())
                return query;


            //Main expression - encapsulates all the partial expressions joined by 'OR' (greedy) or 'AND' (!greedy / exact)
            Expression mainExpression = null;

            //This is the object type the filter will be executed upon; specifies the param name too: as in something like this p => p.something...
            //type will be used to work out the p type, while "p" is the param name
            var expType = Expression.Parameter(targetType, "p");

            //Create expression for each supplied filter
            foreach (var filter in filters)
            {
                Expression filterExpression = null;

                //Check if model property exists; if not this is a bad, bad request...
                var propertyToFilterBy = targetType.GetProperties().FirstOrDefault(p => string.Equals(p.Name, filter.Property, StringComparison.CurrentCultureIgnoreCase));
                if (propertyToFilterBy == null)
                    throw new BadRequestException($"The property {targetType}.{filter.Property} is not defined for the model!");

                //work out what sort of filtering should be applied for a property...

                //string filter
                if (filter.Operator == "like" && filter.Value is string)
                {
                    //ExtJs sends string filters ike this:
                    //{"operator":"like","value":"some value","property":"name"}

                    //make sure the type of the property to filter by is ok
                    if (propertyToFilterBy.PropertyType != typeof(string))
                            throw new BadRequestException($"The property {targetType}.{propertyToFilterBy.Name} is not of 'string' type.");

                    
                    //This will call to lower on the property that the filter applies to;
                    //pretty much means call ToLower on the property - something like p => p.ToLower()
                    var toLower = Expression.Call(
                        Expression.Property(expType, filter.Property), //this specify the property to filter by on the param object specified earlier - so p => p.Property
                        typeof(string).GetMethod("ToLower", Type.EmptyTypes) //this is the method to be called on the property specified above
                    );

                    //finally need to assemble an equivaluent of p.Property.ToLower().Contains(filter.Value)
                    filterExpression = Expression.Call(toLower, "Contains", null, Expression.Constant(((string)filter.Value.ToString()).ToLower(), typeof(string)));
                }

                //range filter
                else if (filter.Operator == "in" && filter.Value is JArray)
                {
                    //{"operator":"in","value":[11,18],"property":"name"}

                    try
                    {
                        var json = ((string)filter.Value.ToString()).Trim().TrimStart('{').TrimEnd('}');
                        var list = JsonConvert.DeserializeObject<List<object>>(json);

                        foreach (var item in list)
                        {
                            Expression inExpression = Expression.Equal(Expression.Property(expType, filter.Property),
                                Expression.Convert(Expression.Constant(item), Expression.Property(expType, filter.Property).Type));

                            //join the filters for each item in a list with OR
                            filterExpression = filterExpression == null ? inExpression : Expression.OrElse(filterExpression, inExpression);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new BadRequestException("Property: " + propertyToFilterBy.Name + ", " + ex.Message, ex.InnerException);
                    }
                }

                //boolean filter
                else if (filter.Operator == "==" && filter.Value is bool)
                {
                    //{"operator":"==","value":true,"property":"name"}

                    if (propertyToFilterBy.PropertyType != typeof(bool))
                            throw new BadRequestException($"The property { targetType }.{ propertyToFilterBy.Name} is not of 'bool' type.");

                    filterExpression = Expression.Call(Expression.Property(expType, filter.Property), "Equals", null, Expression.Constant(filter.Value, typeof(bool)));
                }

                //Lower than / greater than / equals; applies to numbers and dates
                else if ((filter.Operator == "lt" || filter.Operator == "gt" || filter.Operator == "eq")
                    && (filter.Value.IsNumeric() || filter.Value is DateTime))
                {
                    //{"operator":"lt","value":10,"property":"name"}
                    //{"operator":"gt","value":10,"property":"name"}
                    //{"operator":"eq","value":10,"property":"name"}
                    //{"operator":"lt","value":"2016-05-08T22:00:00.000Z","property":"name"}
                    //{"operator":"gt","value":"2016-05-08T22:00:00.000Z","property":"name"}
                    //{"operator":"eq","value":"2016-05-08T22:00:00.000Z","property":"name"}

                    try
                    {
                        switch (filter.Operator)
                        {
                            case "eq":
                                filterExpression = Expression.Equal(Expression.Property(expType, filter.Property),
                            Expression.Convert(Expression.Constant(filter.Value), Expression.Property(expType, filter.Property).Type));
                                break;
                            case "lt":
                                filterExpression = Expression.LessThan(Expression.Property(expType, filter.Property),
                            Expression.Convert(Expression.Constant(filter.Value), Expression.Property(expType, filter.Property).Type));
                                break;
                            case "gt":
                                filterExpression = Expression.GreaterThan(Expression.Property(expType, filter.Property),
                            Expression.Convert(Expression.Constant(filter.Value), Expression.Property(expType, filter.Property).Type));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new BadRequestException("Property: " + propertyToFilterBy.Name + ", " + ex.Message, ex.InnerException);
                    }
                }

                // If no expression generated then throw exception
                if (filterExpression == null)
                    throw new BadRequestException($"Filter operator: {filter.Operator} for type: {filter.Value.GetType()} is not implemented (property: {propertyToFilterBy.Name} should be {propertyToFilterBy.PropertyType})");

                mainExpression = mainExpression == null ? 
                    filterExpression : 
                    greedy ? Expression.OrElse(mainExpression, filterExpression) : Expression.AndAlso(mainExpression, filterExpression);
            }


            // If no expression generated return base query
            if (mainExpression == null)
                return query;

            //assemble a lambda that will be executed for the type in question
            var containsLambda = Expression.Lambda(mainExpression, expType);

            //make a param from the query / expression tree
            var targetAsConstant = Expression.Constant(query, query.GetType());

            //finally assemble the where body; this would translate to something like Where(p => Filtering expression for p value)
            var whereBody = Expression.Call(typeof(Queryable), "Where", new[] { targetType }, targetAsConstant, containsLambda);

            // Return query with filter expressions
            return query.Provider.CreateQuery<T>(whereBody);
        }
    }
}
