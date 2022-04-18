using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class DocumentStateHistory
    {
        [Parameter]
        public string DocumentId { get; set; }

        private IList<DocumentStateDto> _statusHistory;

        protected override async Task OnInitializedAsync()
        {
            _statusHistory = new List<DocumentStateDto>()
            {
                new DocumentStateDto() { Timestamp = DateTime.Now.AddDays(-20),State = "Received" },
                new DocumentStateDto() { Timestamp = DateTime.Now.AddDays(-10),State = "Approved" },
            };
            await base.OnInitializedAsync();
        }
    }
}
