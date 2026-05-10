using ApplicationNews;
using BLAZAM.Database.Services;
using BLAZAM.Jobs;
using BLAZAM.Localization;
using BLAZAM.Logger;
using BLAZAM.Session.Interfaces;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace BLAZAM.Services.Background
{
    [AutoStartBackgroundService(true)]
    public class ApplicationNewsService : DatabaseBackgroundServiceBase, IApplicationNewsService
    {
        private const string _primaryNewsApi = "https://localhost/";
        private const string _secondaryNewsApi = "https://localhost/";
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly HttpClient _httpClient;
        private readonly HttpClient _secondaryHttpClient;
        private bool _pollCompleted = false;
        private List<NewsItem> _allNewsItems = [];
        private IEnumerable<NewsItem> activeNewsItems => _allNewsItems.Where(x => x.DeletedAt == null
                                                                        && x.Published
                                                                        && (x.ScheduledAt == null
                                                                        || x.ScheduledAt < DateTime.Now)
                                                                        && (x.ExpiresAt == null || x.ExpiresAt > DateTime.Now));
        public AppEvent OnNewItemsAvailable { get; set; } = new();

        public ApplicationNewsService(IAppDatabaseFactory dbFactory, IStringLocalizer<AppLocalization> appLocalization) : base(dbFactory, appLocalization)
        {
            Interval = TimeSpan.FromMinutes(15);

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_primaryNewsApi),
                Timeout = TimeSpan.FromSeconds(60)
            };
            _secondaryHttpClient = new HttpClient
            {
                BaseAddress = new Uri(_secondaryNewsApi),
                Timeout = TimeSpan.FromSeconds(60)
            };
        }


        protected override void Execute(object? state = null)
        {
            // News feed from upstream provider is disabled in this fork.
            // To re-enable, restore the HTTP client calls to the upstream news API.
            _pollCompleted = true;
        }

        private async Task<bool> GetNewsAsync(HttpClient httpClient)
        {
            var apiResponse = await httpClient.GetAsync("newsItems");
            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                var content = await apiResponse.Content.ReadAsStringAsync();
                var allNewsItems = JsonSerializer.Deserialize<List<NewsItem>>(content, _jsonOptions);
                if (allNewsItems != null)
                {
                    _allNewsItems = allNewsItems;
                    _pollCompleted = true;

                    OnNewItemsAvailable?.Invoke();

                }

                return true;
            }

            throw new AppException("News API did not return a successful response.");
        }

        public List<NewsItem> GetUnreadNewsItems(IApplicationUserState user)
        {
            try
            {
                var activeItems = activeNewsItems;
                var unreadItems = new List<NewsItem>();

                if (user?.ReadNewsItems == null)
                {
                    return activeItems.ToList();
                }

                var readNewsItems = user.ReadNewsItems;
                var readIds = readNewsItems.Select(x => x.NewsItemId).ToHashSet();
                var updatedReadItems = readNewsItems
                    .Where(r => activeItems.Any(a => a.Id.Equals(r.NewsItemId) && r.NewsItemUpdatedAt < a.UpdatedAt))
                    .Select(r => r.NewsItemId)
                    .ToHashSet();

                foreach (var item in activeItems)
                {
                    bool isRead = readIds.Contains(item.Id);
                    bool isUpdated = updatedReadItems.Contains(item.Id);

                    if (!isRead || isUpdated)
                    {
                        unreadItems.Add(item);
                    }
                }

                // Clean up stale read items that are no longer active
                if (_pollCompleted)
                {
                    var staleItems = readNewsItems
                        .Where(x => x.NewsItemId < 100000000000 && !activeItems.Any(a => a.Id.Equals((ulong)x.NewsItemId)))
                        .ToList();

                    if (staleItems.Count > 0)
                    {
                        foreach (var x in staleItems)
                        {
                            readNewsItems.Remove(x);
                        }

                        user.SaveReadNewsItems();
                    }
                }

                return unreadItems;
            }
            catch (Exception ex)
            {
                Loggers.SystemLogger.Error(ex, "Error while trying to get unread news items for user.");
                return [];
            }
        }


        public List<NewsItem> GetReadNewsItems(IApplicationUserState user)
        {
            try
            {
                if (user != null)
                {
                    var activeItems = activeNewsItems;
                    if (user.ReadNewsItems != null)
                    {
                        var readItems = activeItems.Where(x => user.ReadNewsItems.Any(r => r.NewsItemId == x.Id && r.NewsItemUpdatedAt >= x.UpdatedAt)).ToList();

                        return readItems;
                    }

                    return [];
                }

                return [];

            }
            catch (Exception ex)
            {
                Loggers.SystemLogger.Error(ex, "Error while trying to get read news items for user.");
                return [];
            }
        }
        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                _httpClient.Dispose();
                _secondaryHttpClient.Dispose();
            }



            base.Dispose(disposing);
        }

    }
}
