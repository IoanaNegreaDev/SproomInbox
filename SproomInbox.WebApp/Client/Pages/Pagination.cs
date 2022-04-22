using Microsoft.AspNetCore.Components;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
namespace SproomInbox.WebApp.Client.Pages
{
    public partial class Pagination
    {
        [Parameter]
        public PagedListMetadata MetaData { get; set; }

        [Parameter]
        public int Spread { get; set; }

        [Parameter]
        public EventCallback<int> SelectedPage { get; set; }

        private List<PagingLink> _links;

        protected override void OnParametersSet()
        {
            CreatePaginationLinks();
        }

        private void CreatePaginationLinks()
        {
            _links = new List<PagingLink>();

            _links.Add(new PagingLink(MetaData.Current - 1, MetaData.HasPrevious, "Previous"));
            for (int i = 1; i <= MetaData.TotalPages; i++)
            {
                if (i >= MetaData.Current - Spread && i <= MetaData.Current + Spread)
                {
                    _links.Add(new PagingLink(i, true, i.ToString()) { Active = MetaData.Current == i });
                }
            }
            _links.Add(new PagingLink(MetaData.Current + 1, MetaData.HasNext, "Next"));
        }

        private async Task OnSelectedPage(PagingLink link)
        {
            if (link.Page == MetaData.Current || !link.Enabled)
                return;

            MetaData.Current = link.Page;
            CreatePaginationLinks();
            await SelectedPage.InvokeAsync(link.Page);
        }
    }
}