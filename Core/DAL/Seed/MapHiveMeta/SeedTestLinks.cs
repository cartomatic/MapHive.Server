using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DAL.Seed.MapHiveMeta
{
    public partial class Seed
    {
        public static void SeedTestLinks(MapHiveDbContext context)
        {
#if DEBUG
            try
            {
                var l = new Link
                {
                    ParentTypeUuid = default(Guid),
                    ChildTypeUuid = default(Guid),
                    ParentUuid = default(Guid),
                    ChildUuid = default(Guid)
                };

                l.LinkData.Add("some_link_data_consumer", new Dictionary<string, object>
                {
                    {"prop1", "Some textual property"},
                    {"prop2", 123},
                    {"prop3", DateTime.Now}
                });

                context.Links.AddOrUpdate(
                    l,
                    new Link
                    {
                        ParentTypeUuid = default(Guid),
                        ChildTypeUuid = default(Guid),
                        ParentUuid = default(Guid),
                        ChildUuid = default(Guid),
                        LinkData = new LinkData
                        {
                            {
                                "will_this_nicely_serialize", new Dictionary<string, object>
                                {
                                    {"prop1", new Application()},
                                    {"prop2", new Link()}
                                }
                            }
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.WriteAllText(@"f:\err.txt", ex.Message + Environment.NewLine + ex.StackTrace);
                }
                catch
                {
                    //ignore
                }
            }
#endif
        }
    }
}
