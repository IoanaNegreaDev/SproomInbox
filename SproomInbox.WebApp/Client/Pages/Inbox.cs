using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources;
using System.Net.Http.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class Inbox
    {
        private IList<DocumentDto>? _documents;
        private List<string> _selectedIds { get; set; } = new List<string>();

        [Parameter]
        public string UserNameFilter{ get; set; } = string.Empty;

        [Parameter]
        public string DocumentStateFilter { get; set; } = string.Empty;

        [Parameter]
        public string DocumentTypeFilter { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserNameFilter))
                return;

            _documents = await OnRefreshDocumentsAsync();
            await base.OnInitializedAsync();
        }

        private async Task<IList<DocumentDto>> OnRefreshDocumentsAsync()
        {
            var filterString = $"?username={UserNameFilter}";
            filterString += $"&type={DocumentTypeFilter}";
            filterString += $"&state={DocumentStateFilter}";

            return await Http.GetFromJsonAsync<IList<DocumentDto>>($"document" + filterString);
        }

        private async Task<IList<DocumentDto>> OnApprove()
        {
            var newState = Enum.GetName<StateDto>(StateDto.Approved);
            var updateParameters = new DocumentListUpdateParameters()
            {
                DocumentIds = _selectedIds,
                UserName = UserNameFilter,
                NewState = newState
            };

            var response = await Http.PutAsJsonAsync<DocumentListUpdateParameters>($"document", updateParameters);

            if (response.IsSuccessStatusCode)
                foreach (var document in _documents?.Where(item => _selectedIds.Contains(item.Id)).ToList())
                    document.State = newState;
            StateHasChanged();
            return _documents;
        }
        private async Task<IList<DocumentDto>> OnReject()
        {
            var newState = Enum.GetName<StateDto>(StateDto.Rejected);

            var updateParameters = new DocumentListUpdateParameters()
            {
                DocumentIds = _selectedIds,
                UserName = UserNameFilter,
                NewState = newState
            };

            var response = await Http.PutAsJsonAsync<DocumentListUpdateParameters>($"document", updateParameters);

            if (response.IsSuccessStatusCode)
                foreach (var document in _documents?.Where(item => _selectedIds.Contains(item.Id)).ToList())
                    document.State = newState;
            StateHasChanged();
            return _documents;
        }

        private bool IsVisible(DocumentDto document)
            =>  document.State.ToLower().Contains(DocumentStateFilter.ToLower()) ||
                document.Type.ToLower().Contains(DocumentTypeFilter.ToLower());
              
        private bool IsDocumentFinalState(string state)
        {
            if (Enum.TryParse<StateDto>(state, out var stateValue))
                if (stateValue != StateDto.Received)
                    return true;
            return false;
        }

        private void OnCheckboxClicked(string documentId, object isChecked)
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
