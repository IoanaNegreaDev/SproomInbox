using System.ComponentModel;

namespace SproomInbox.WebApp.Shared.Resources
{
    /// <summary>
    /// The history of state change of a document
    /// </summary>
    public class DocumentStateDto
    {
        /// <summary>
        /// The setup time for the state
        /// </summary>
        [Description("The setup time for the state")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The state of a document: Received, Approved, Rejected
        /// </summary>
        [Description("The state of a document")]
        public string State { get; set; } = String.Empty;
    }
}
