using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources;
using System.Net.Http.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class Inbox
    {
        private IList<DocumentDto>? _documents;

        private IList<UserDto>? _users;
        [Parameter]
        public string UserNameFilter { get; set; }

        [Parameter]
        public string DocumentStateFilter { get; set; }

        [Parameter]
        public string DocumentTypeFilter { get; set; }

        public DocumentDto Document { get; set; } = new DocumentDto();

        //    public StateDto DocumentState { get; set; } = new StateDto();
        //    public DocumentTypeDto DocumentType { get; set; } = new DocumentTypeDto();
        //   public List<string> SelectedValues { get; set; } = new List<string>();
        protected override async Task OnInitializedAsync()
        {
            Console.Write($"STATAEEEEEEEEEEEEEEEEEEEE : {UserNameFilter}");
            _documents = await OnRefreshDocumentsAsync();
            Console.Write($"STATAEEEEEEEEEEEEEEEEEEEE : {DocumentStateFilter}");
            await base.OnInitializedAsync();
        }

        protected async Task<IList<DocumentDto>> OnRefreshDocumentsAsync()
        {
            string filterString = string.Empty;
            if (!string.IsNullOrEmpty(UserNameFilter))
                filterString = $"?username={UserNameFilter}";

    
                filterString += $"&type={DocumentTypeFilter}";

       
                    filterString += $"&state={DocumentStateFilter}";

            return await Http.GetFromJsonAsync<IList<DocumentDto>>($"document" + filterString);
        }
     

        public bool IsVisible(DocumentDto document)
        {      
            Console.Write($"state : {DocumentStateFilter}");
            bool visible = document.State.ToLower().Contains(DocumentStateFilter.ToLower()) ||
                           document.Type.ToLower().Contains(DocumentTypeFilter.ToLower());
              
            return visible;
        }

        /*        public void CheckboxClicked(string aSelectedId, object aChecked)
                {
                    if ((bool)aChecked)
                    {
                        if (!SelectedValues.Contains(aSelectedId))
                        {
                            SelectedValues.Add(aSelectedId);
                        }
                    }
                   else
                    {
                        if (SelectedValues.Contains(aSelectedId))
                        {
                            SelectedValues.Remove(aSelectedId);
                        }
                    }
                    StateHasChanged();
                }*/


    }
}
