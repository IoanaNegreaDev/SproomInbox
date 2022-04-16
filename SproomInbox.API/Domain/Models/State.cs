using System.ComponentModel;

namespace SproomInbox.API.Domain.Models
{
    public enum State : byte
    {
        [Description("Received")]
        Received = 1,

        [Description("Approved")]
        Approved ,

        [Description("Rejected")]
        Rejected ,
    }
}
