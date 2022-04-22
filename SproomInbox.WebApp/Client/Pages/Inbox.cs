using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Client.Services;
using SproomInbox.WebApp.Shared.Pagination;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Net.Http.Json;
using System.Text.Json;

namespace SproomInbox.WebApp.Client.Pages
{
    public partial class Inbox
    {
        [Inject]
        public IDocumentsFromWebServerService DocumentService { get; set; }

        [Parameter]
        public DocumentsQueryParameters FilterParameters { get; set; } = new DocumentsQueryParameters();
  
        private IList<DocumentDto> _documents = new List<DocumentDto>();
        public PagedListMetadata PaginationMetaData { get; set; } = new PagedListMetadata();
        private List<Guid> _selectedIds { get; set; } = new List<Guid>();
        private Dictionary<Guid, bool> _documentExpanded = new Dictionary<Guid, bool>();
        private Dictionary<Guid, bool> _documentChecked = new Dictionary<Guid, bool>();
        private bool _failedToUpdate = false;

        protected override async Task OnInitializedAsync()
        {
            await RefreshInbox();
            await base.OnInitializedAsync();
        }

        private async Task RefreshInbox()
        {
            if (string.IsNullOrEmpty(FilterParameters.UserName))
                return;

            var response = await DocumentService.FetchDocumentsAsync(FilterParameters);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _documents = await response.Content.ReadFromJsonAsync<List<DocumentDto>>() ?? new List<DocumentDto>();

                PaginationMetaData = JsonSerializer.Deserialize<PagedListMetadata>(response.Headers.GetValues("X-Pagination").First());

                ResetData();
            }
        }

        private void ResetData()
        {
            _documentChecked.Clear();
            _documentExpanded.Clear();
            _selectedIds.Clear();

            bool documentExpanded = false;
            bool documentChecked = false;
            foreach (var document in _documents)
            {
                _documentExpanded.Add(document.Id, documentExpanded);
                _documentChecked.Add(document.Id, documentChecked);
            }
        }
        private void OnCheckboxClicked(Guid documentId, object isChecked)
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
            if (!response.IsSuccessStatusCode)
                _failedToUpdate = true;

            await RefreshInbox();
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
            if (!response.IsSuccessStatusCode)
                _failedToUpdate = true;

            await RefreshInbox();
            StateHasChanged();
        }
        private async Task<HttpResponseMessage> UpdateDocumentsAsync(string newState)
        {
            var updateParameters = new DocumentsUpdateStatusParameters()
            {
                DocumentIds = _selectedIds,
                UserName = FilterParameters.UserName,
                NewState = newState
            };

            return await DocumentService.UpdateDocumentsAsync(updateParameters);
        }

        private async Task SelectedPage(int page)
        {
            FilterParameters.Page.Current = page;
            await RefreshInbox();
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
