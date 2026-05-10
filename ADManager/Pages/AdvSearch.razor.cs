// Import necessary namespaces for various functionalities
using ADManager.ActiveDirectory.Adapters;
using ADManager.ActiveDirectory.Interfaces;
using ADManager.ActiveDirectory.Searchers;
using ADManager.Common.Data;
using ADManager.Gui.Layouts;
using ADManager.Gui.UI;
using ADManager.Gui.UI.Outputs;
using ADManager.Services;
using ADManager.Services.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics;
using System.Web;

namespace ADManager.Pages
{
    /// <summary>
    /// Represents a search component that facilitates searching for directory entries based on various parameters and
    /// filters.
    /// </summary>
    /// <remarks>This component integrates with the <see cref="SearchService"/> to manage search parameters
    /// and performs searches using the <see cref="ADSearch"/> class. It supports initializing search terms from the
    /// URI, handling cascading parameters, and managing search results.</remarks>
    public partial class AdvSearch : SearchPageBase
    {
        [Parameter]
        public string? seachGuid { get; set; }

        private string? _previousSeachGuid;

        protected override async Task OnParametersSetAsync()
        {
            if (_previousSeachGuid != seachGuid)
            {
                _previousSeachGuid = seachGuid;
                await PerformSearch();
            }
        }
        protected override async Task PerformSearch()
        {
            LoadingData = true;
            if (SearchService.Filters != null && SearchService.Filters.Count > 0)
            {
                Searcher.Results = await SearchService.Filters.GetFilteredEntries(SearchService.SeachObjectType, DbFactory, Directory);
            }
            LoadingData = false;
        }
    }
}