using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Client.Services;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Net.Http.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class FilteredInbox
    {
        [Inject]
        public IUsersFromWebServerService UserService { get; set; }

        private IList<UserDto>? _users;
        private DocumentListQueryParameters FilterParameters { get; set; } = new DocumentListQueryParameters();
        protected override async Task OnInitializedAsync()
        {

            var response = await UserService.FetchUsersAsync();
            if (response.IsSuccessStatusCode)
                _users = await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? new List<UserDto>();
            await base.OnInitializedAsync(); ;
        }  
    }
}
