using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    /// <summary>
    /// Provides utils for type identifier management; in order to be abe to link objects freely, each object needs to have a unique type identifier
    /// </summary>
    public static partial class BaseObjectTypeIdentifierExtensions
    {
        /// <summary>
        /// Maps types to their declared type identifiers
        /// </summary>
        private static Dictionary<Type, Guid> TypesToTypeIdentifiers = new Dictionary<Type, Guid>();

        /// <summary>
        /// maps type identifiers to their types
        /// </summary>
        private static Dictionary<Guid, Type> TypeIdentifiersToTypes = new Dictionary<Guid, Type>();

        /// <summary>
        /// registers type identifier in the runtime cache
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uuid"></param>
        public static void RegisterTypeIdentifier(Type type, Guid uuid)
        {
            if (TypesToTypeIdentifiers.ContainsKey(type) && TypesToTypeIdentifiers[type] != uuid)
            {
                //for the time being DO not throw...
                //throw new Exception($"Type being registered is not unique. It is not allowed to register a type more than once. Type: {type}, identifier: {uuid}");
                try
                {
                    var dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_errorlog");
                    if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }

                    System.IO.File.AppendAllLines(System.IO.Path.Combine(dir, $"{DateTime.Now:yyyy-MM-dd}_type-registration.log"), new[]
                    {
                            $"TypeToTypeIdentifier duplicate err",
                            $"Type: {type?.FullName}; old uuid: {TypesToTypeIdentifiers[type]}; new uuid {uuid}",
                            new string('-',50),
                            Environment.NewLine
                        });
                }
                catch { }
            }

            TypesToTypeIdentifiers[type] = uuid;


            if (TypeIdentifiersToTypes.ContainsKey(uuid) && TypeIdentifiersToTypes[uuid] != type)
            {
                //for the time being DO not throw...
                //throw new Exception($"Type identifier being registered is not unique. Please provide a unique identifier for a type. Identifier: {uuid}, type: {type}");

                try
                {
                    var dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_errorlog");
                    if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }

                    System.IO.File.AppendAllLines(System.IO.Path.Combine(dir, $"{DateTime.Now:yyyy-MM-dd}_type-registration.log"), new[]
                    {
                            $"TypeIdentifiersToTypes duplicate err",
                            $"Uuid: {uuid}; old type: {TypeIdentifiersToTypes[uuid].FullName}; new uuid {type?.FullName}",
                            new string('-',50),
                            Environment.NewLine
                        });
                }
                catch { }
            }

            TypeIdentifiersToTypes[uuid] = type;
        }

        /// <summary>
        /// Gets an identifier for a specified type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Guid GetTypeIdentifier(Type t)
        {
            //fix for the EF proxies...
            //if an object is proxied, need get the underlying object!!!
            if (t.FullName.IndexOf("System.Data.Entity.DynamicProxies") > -1)
            {
                t = t.BaseType;
            }

            try
            {
                return TypesToTypeIdentifiers[t];
            }
            catch
            {
                throw new Exception($"Type [{t}] is not registered. This is usually done via static ctor. Please see any subclass of MapHive.Server.Core.DataModel.Base for details.");
            }
        }


        /// <summary>
        /// Gets an identifier for a specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Guid GetTypeIdentifier<T>(this T obj)
            where T: Base
        {
            return GetTypeIdentifier(obj.GetType());
        }

        /// <summary>
        /// Gets an identifier for a specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Guid GetTypeIdentifier<T>()
        {
            return GetTypeIdentifier(typeof(T));
        }

        /// <summary>
        /// Gets type by its identifier 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Type GetTypeByIdentifier(Guid identifier)
        {
            try
            {
                return TypeIdentifiersToTypes[identifier];
            }
            catch
            {
                throw new Exception($"There is no type registered for [{identifier}]. Type identifier registration is usually done via static ctor. Please see any subclass of MapHive.Server.Core.DataModel.Base for details.");
            }
        }

        /// <summary>
        /// Automatically registers all the types subclassing MapHive.Server.Core.DataModel.Base
        /// </summary>
        public static void AutoRegisterBaseTypes()
        {
            //simply get the registered types. in order to decide whether a type is registered or not it gets registered through a static ctor
            GetRegisteredBaseSubclassingTypes();
        }

        /// <summary>
        /// gets a collection of registered types
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetRegisteredBaseSubclassingTypes()
        {
            var registered = new List<Type>();

            foreach (var type in GetBaseSubclassingTypes().Where(t=>!t.IsAbstract))
            {
                //type registration is performed via static ctors, so just need to poke the class to fire it up

                try
                {
                    //try to create an instance and then obtain its type id
                    Activator.CreateInstance(type);
                    GetTypeIdentifier(type); //this will throw if a type is not registered

                    registered.Add(type);
                }
                catch
                {
                    //ignore
                }

            }

            return registered;
        }

        /// <summary>
        /// gets all the types subclassing MapHive.Server.Core.DataModel.Base 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetBaseSubclassingTypes()
        {
            var subTypes = new List<Type>();
            var baseType = typeof(Base);

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    //gettypes tends to throw if an assembly cannot be loaded
                    foreach (var type in a.GetTypes().Where(t=>t.IsSubclassOf(baseType)))
                    {
                        if (!subTypes.Contains(type))
                        {
                            subTypes.Add(type);
                        }
                    }
                }
                catch
                {
                    //ignore
                }
            }
            return subTypes;
        }

    }
}
