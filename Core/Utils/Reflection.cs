using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Utils
{
    public static class Reflection
    {
        /// <summary>
        /// Returns property memebr info - extracts data off an expression that specifies a property
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetPropertyMemberInfoFromExpression<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            var memberExp = expression.Body as MemberExpression;

            if (memberExp == null)
                throw new ArgumentException("Member does not exist.");

            var member = memberExp.Member;

            if (member.MemberType != MemberTypes.Property)
                throw new ArgumentException("Member is not a property.");

            return member;
        }
    }
}
