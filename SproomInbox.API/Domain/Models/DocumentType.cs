using System.ComponentModel;

namespace SproomInbox.API.Domain.Models
{
    public enum DocumentType : byte
    {
        [Description("Invoice")]
        Invoice = 1,

        [Description("CreditNote")]
        CreditNote,
    }
}
