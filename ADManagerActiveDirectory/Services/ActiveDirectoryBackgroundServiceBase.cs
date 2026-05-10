using ADManager.ActiveDirectory.Interfaces;
using ADManager.Database.Services;
using ADManager.Localization;
using Microsoft.Extensions.Localization;

namespace ADManager.ActiveDirectory.Services
{
    public class ActiveDirectoryBackgroundServiceBase : DatabaseBackgroundServiceBase
    {
        protected readonly IActiveDirectoryContextFactory activeDirectoryContextFactory;

        public ActiveDirectoryBackgroundServiceBase(IActiveDirectoryContextFactory activeDirectoryContextFactory, IAppDatabaseFactory dbFactory, IStringLocalizer<AppLocalization> appLocalization) : base(dbFactory, appLocalization)
        {
            this.activeDirectoryContextFactory = activeDirectoryContextFactory;
        }
    }
}
