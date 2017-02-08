using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel.Image
{
    public partial class Image
    {
        /// <summary>
        /// Updates a img and returns an id
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="imgId"></param>
        /// <param name="imgData"></param>
        /// <returns></returns>
        public static async Task<Guid?> Update(Guid objectId, Guid? imgId, string imgData)
        {
            //object id is the id of an object the image applies to
            //when no img guid, but img data then generate and save img.
            //when img guid and img data then update

            //when no img guid and no imgData then delete

            return default(Guid);
        }
    }
}
