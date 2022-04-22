using System.ComponentModel;

namespace SproomInbox.WebApp.Shared.Resources
{
    /// <summary>
    /// The transfer reprezentation of a user
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The username of a user that accesses the documents
        /// </summary>
        [Description("The username of a user that accesses the documents")]
        public string UserName { get; set; } = String.Empty;
        /// <summary>
        /// The first name  of a user that accesses the documents
        /// </summary>
        [Description("The first name of a user that accesses the documents")]
        public string FirstName { get; set; } = String.Empty;
        /// <summary>
        /// The last name  of a user that accesses the documents
        /// </summary>
        [Description("The last name of a user that accesses the documents")]
        public string LastName { get; set; } = String.Empty;
    }
}
