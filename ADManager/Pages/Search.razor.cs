// Import necessary namespaces for various functionalities
using ADManager.ActiveDirectory.Adapters;
using ADManager.ActiveDirectory.Interfaces;
using ADManager.ActiveDirectory.Searchers;
using ADManager.Common.Data;
using ADManager.Gui.Layouts;
using ADManager.Gui.UI;
using ADManager.Gui.UI.Outputs;
using ADManager.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
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
    public partial class Search : SearchPageBase
    {
      
    }
}