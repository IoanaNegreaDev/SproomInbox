using SproomInbox.API.Utils.Parametrization;
using SproomInbox.WebApp.Shared.Resources;
using System.Net.Http.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class FilteredInbox
    {
        private IList<UserDto>? _users;
        private DocumentListQueryParameters FilterParameters { get; set; } = new DocumentListQueryParameters();
        protected override async Task OnInitializedAsync()
        {
            _users = await Http.GetFromJsonAsync<IList<UserDto>>($"users");  
            await base.OnInitializedAsync(); ;
        }  
    }
}
