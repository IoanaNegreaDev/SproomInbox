using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources;
using System.Net.Http.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class Inbox
    {
        private IList<DocumentDto>? _documents;
        private IList<string> _selectedIds { get; set; } = new List<string>();
        private DocumentDto Document { get; set; } = new DocumentDto();

        private IList<UserDto>? _users;
        [Parameter]
        public string UserNameFilter { get; set; } = string.Empty;

        [Parameter]
        public string DocumentStateFilter { get; set; } = string.Empty;

        [Parameter]
        public string DocumentTypeFilter { get; set; } = string.Empty;


        //    public StateDto DocumentState { get; set; } = new StateDto();
        //    public DocumentTypeDto DocumentType { get; set; } = new DocumentTypeDto();
        //   public List<string> SelectedValues { get; set; } = new List<string>();
        protected override async Task OnInitializedAsync()
        {
             _documents = await OnRefreshDocumentsAsync();
            await base.OnInitializedAsync();
        }

        protected async Task<IList<DocumentDto>> OnRefreshDocumentsAsync()
        {
            string filterString = string.Empty;
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

        public void OnCheckboxClicked(string documentId, object isChecked)
        {
            if ((bool)isChecked)
            {
                if (!_selectedIds.Contains(documentId))
                {
                    _selectedIds.Add(documentId);
                }
            }
            else
            {
                if (_selectedIds.Contains(documentId))
                {
                    _selectedIds.Remove(documentId);
                }
            }
            StateHasChanged();
        }
    }
}
