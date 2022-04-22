using System.ComponentModel;

namespace SproomInbox.WebApp.Shared.Resources
{
    /// <summary>
    /// The available states for a document
    /// </summary>
    public enum StateDto : byte
    {
        [Description("Received")]
        Received = 1,

        [Description("Approved")]
        Approved,

        [Description("Rejected")]
        Rejected 
    }
}
