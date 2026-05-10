using ADManager.Localization;
using ADManager.Services.Background;
using Microsoft.Extensions.Localization;

namespace ADManager.Database.Services
{
    public class DatabaseBackgroundServiceBase : BackgroundServiceBase
    {
        protected readonly IAppDatabaseFactory dbFactory;

        public DatabaseBackgroundServiceBase(IAppDatabaseFactory dbFactory, IStringLocalizer<AppLocalization> appLocalization) : base(appLocalization)
        {
            this.dbFactory = dbFactory;

        }
    }
}
