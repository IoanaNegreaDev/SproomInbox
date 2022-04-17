using System.ComponentModel;

namespace SproomInbox.WebApp.Shared.Resources
{
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
