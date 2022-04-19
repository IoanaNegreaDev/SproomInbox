using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class DocumentStateHistory
    {
        [Parameter]
        public DocumentDto Document { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
