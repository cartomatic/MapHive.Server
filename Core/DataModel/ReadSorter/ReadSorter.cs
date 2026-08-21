namespace MapHive.Server.Core.DataModel
{
    public partial class ReadSorter
    {
        /// <summary>
        /// Object property to sort on
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// ummm.... well... sort direction ;)
        /// </summary>
        public string Direction { get; set; }
    }
}
