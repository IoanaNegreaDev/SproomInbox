using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class Filters
    {
        private IList<UserDto>? _users;
        public string UserNameFilter { get; set; } = String.Empty;
        public string DocumentStateFilter { get; set; } = String.Empty;
        public string DocumentTypeFilter { get; set; } = String.Empty;

        public DocumentDto Document { get; set; } = new DocumentDto();

        [Parameter]
        public EventCallback<int> OnFiltersChange { get; set; }
        protected override async Task OnInitializedAsync()
        {
           
            _users = new List<UserDto>()
            {
                new UserDto()
                {
                    UserName = "MuadDib",
                    FirstName = "Paul",
                    LastName = "Atreides"
                },

                new UserDto()
                {
                    UserName = "Hobbit",
                    FirstName = "Frodo",
                    LastName = "Baggins"
                }
            };

            UserNameFilter = _users[0].UserName;
        }  
    }
}
