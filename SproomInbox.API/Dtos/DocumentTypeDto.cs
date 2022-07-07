using System.ComponentModel;

namespace SproomInbox.WebApp.Shared.Resources
{
    /// <summary>
    /// The available types of a document
    /// </summary>
    public enum DocumentTypeDto : byte
    {
        [Description("Invoice")]
        Invoice = 1,

        [Description("CreditNote")]
        CreditNote
    }
}
