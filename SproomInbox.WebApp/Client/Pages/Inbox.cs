using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Client.Services;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Net.Http.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class Inbox
    {
        [Inject]
        public IDocumentsFromWebServerService DocumentService { get; set; }

        [Parameter]
        public DocumentListQueryParameters FilterParameters { get; set; } = new DocumentListQueryParameters();
  
        private IList<DocumentDto> _documents = new List<DocumentDto>();
        private List<string> _selectedIds { get; set; } = new List<string>();
        private Dictionary<string, bool> _documentExpanded = new Dictionary<string, bool>();
        private Dictionary<string, bool> _documentChecked = new Dictionary<string, bool>();

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(FilterParameters.UserName))
                return;

            var response = await DocumentService.FetchDocumentsAsync(FilterParameters);
            if (response.IsSuccessStatusCode)
                _documents = await response.Content.ReadFromJsonAsync<List<DocumentDto>>() ?? new List<DocumentDto>();

            bool documentExpanded = false;
            bool documentChecked = false;
            foreach (var document in _documents)
            {
                _documentExpanded.Add(document.Id, documentExpanded);
                _documentChecked.Add(document.Id, documentChecked);
            }

            await base.OnInitializedAsync();
        }
        private void OnCheckboxClicked(string documentId, object isChecked)
        {
            _documentChecked[documentId] = (bool)isChecked;
            if ((bool)isChecked)
            {
                if (!_selectedIds.Contains(documentId))
                    _selectedIds.Add(documentId);
            }
            else
            {
                if (_selectedIds.Contains(documentId))
                    _selectedIds.Remove(documentId);
            }

            StateHasChanged();
        }
        private async Task OnApprove()
        {
            if (string.IsNullOrEmpty(FilterParameters.UserName))
                return;

            if (_selectedIds == null || _selectedIds.Count <= 0)
                return;

            var newState = Enum.GetName<StateDto>(StateDto.Approved);
            var response = await UpdateDocumentsAsync(newState);

            response = await DocumentService.FetchDocumentsAsync(FilterParameters);
            _documents = await response.Content.ReadFromJsonAsync<List<DocumentDto>>() ?? new List<DocumentDto>();

            foreach (var documentId in _documentChecked.Keys)
                _documentChecked[documentId] = false;

            _selectedIds.Clear();
            StateHasChanged();
        }
        private async Task OnReject()
        {
            if (string.IsNullOrEmpty(FilterParameters.UserName))
                return;

            if (_selectedIds == null || _selectedIds.Count <= 0)
                return;

            var newState = Enum.GetName<StateDto>(StateDto.Rejected);

            var response = await UpdateDocumentsAsync(newState);

            response = await DocumentService.FetchDocumentsAsync(FilterParameters);
            if (response.IsSuccessStatusCode)
                _documents = await response.Content.ReadFromJsonAsync<List<DocumentDto>>() ?? new List<DocumentDto>();

            foreach (var documentId in _documentChecked.Keys)
                _documentChecked[documentId] = false;

            _selectedIds.Clear();
            StateHasChanged();
        }
        private async Task<HttpResponseMessage> UpdateDocumentsAsync(string newState)
        {
            var updateParameters = new DocumentListStatusUpdateParameters()
            {
                DocumentIds = _selectedIds,
                UserName = FilterParameters.UserName,
                NewState = newState
            };

            return await DocumentService.UpdateDocumentsAsync(updateParameters);
        }
  
        private void UpdateDocumentsLocally(string newState)
        {
            var documentsToUpdate = _documents?.Where(item => _selectedIds.Contains(item.Id)).ToList();
            foreach (var document in documentsToUpdate)
                document.State = newState;          
        }
      
        private bool IsDocumentFinalState(string state)
        {
            if (Enum.TryParse<StateDto>(state, out var stateValue))
                if (stateValue != StateDto.Received)
                    return true;

            return false;
        } 
    }
}
