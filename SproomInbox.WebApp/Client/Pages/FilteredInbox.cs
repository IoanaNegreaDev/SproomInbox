using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources;
using System.Net.Http.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class FilteredInbox
    {
        private IList<UserDto>? _users;
        public string UserNameFilter { get; set; } = String.Empty;
        public string DocumentStateFilter { get; set; } = String.Empty;
        public string DocumentTypeFilter { get; set; } = String.Empty;

        public DocumentDto Document { get; set; } = new DocumentDto();

        protected override async Task OnInitializedAsync()
        {
            _users = await Http.GetFromJsonAsync<IList<UserDto>>($"user");  
            await base.OnInitializedAsync(); ;
        }  
    }
}
