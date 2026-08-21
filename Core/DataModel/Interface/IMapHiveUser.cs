namespace MapHive.Server.Core.DataModel.Interface
{
    public interface IMapHiveUser : IBase
    {
        /// <summary>
        /// User's email. Email must be unique in the system and is also a username
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// MembershipReoot's equivalent of IsAccountClosed
        /// </summary>
        bool IsAccountClosed { get; set; }

        /// <summary>
        /// MembershipReoot's equivalent of IsAccountVerified
        /// </summary>
        bool IsAccountVerified { get; set; }
    }
}
