using System.ComponentModel;

namespace SproomInbox.WebApp.Shared.Resources
{
    public enum DocumentTypeDto : byte
    {
        [Description("Invoice")]
        Invoice = 1,

        [Description("CreditNote")]
        CreditNote
    }
}
