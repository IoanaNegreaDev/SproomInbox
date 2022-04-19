using Microsoft.AspNetCore.Components;
using SproomInbox.API.Utils.Parametrization;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;

namespace SproomInbox.WebApp.Client.Pages
{

    public partial class Inbox
    {
        private IList<DocumentDto>? _documents;
        private List<string> _selectedIds { get; set; } = new List<string>();

        private Dictionary<string, bool> _documentView = new Dictionary<string, bool>();

        [Parameter]
        public DocumentListQueryParameters FilterParameters { get; set; } = new DocumentListQueryParameters();

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(FilterParameters.UserName))
                return;

            _documents = await OnFetchDocumentsAsync();

            bool documentExpanded = false;
            foreach (var document in _documents)
                _documentView.Add(document.Id, documentExpanded);

            await base.OnInitializedAsync();
        }

        private async Task<IList<DocumentDto>> OnFetchDocumentsAsync()
        {
           /* var filterString = $"?username={FilterParameters.UserName}";
            filterString += $"&type={FilterParameters.Type}";
            filterString += $"&state={FilterParameters.State}";*/

            NameValueCollection queryPairs = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryPairs.Add("username", FilterParameters.UserName);
            queryPairs.Add("type", FilterParameters.Type);
            queryPairs.Add("state", FilterParameters.State);

            string query = string.Empty;
            if (queryPairs.Count > 0)
                query = "?" + queryPairs.ToString();

            return await Http.GetFromJsonAsync<IList<DocumentDto>>($"documents" + query);
        }
        public static class MemberInfoGetting
        {
            public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
            {
                MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
                return expressionBody.Member.Name;
            }
        }
        private async Task<HttpResponseMessage> UpdateDocumentsAsync(string newState)
        {
            var updateParameters = new DocumentListStatusUpdateParameters()
            {
                DocumentIds = _selectedIds,
                UserName = FilterParameters.UserName,
                NewState = newState
            };

            return await Http.PutAsJsonAsync<DocumentListStatusUpdateParameters>($"documents", updateParameters);
        }

        private async Task OnApprove()
        {
            if (string.IsNullOrEmpty(FilterParameters.UserName))
                return;

            if (_selectedIds == null || _selectedIds.Count <=0)
                return;

            var newState = Enum.GetName<StateDto>(StateDto.Approved);
            var response = await UpdateDocumentsAsync(newState);

            if (response.IsSuccessStatusCode)
                UpdateDocumentsLocally(newState);

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

            if (response.IsSuccessStatusCode)
                UpdateDocumentsLocally(newState);

            StateHasChanged();
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

        private void OnCheckboxClicked(string documentId, object isChecked)
        {
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
    }
}
