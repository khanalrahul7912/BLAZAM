using ADManager.ActiveDirectory.Interfaces;
using ADManager.Database.Context;
using ADManager.Services;
using ADManager.Services.Background;
using ADManager.Session;
using ADManager.Session.Interfaces;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADManager.Helpers
{
    public static class UserStateServiceExtentions
    {
        public static async Task<IApplicationUserState?> GetApplicationUser(this IDirectoryEntryAdapter entry, IApplicationUserStateService userStateService,
            IAppDatabaseFactory appDatabaseFactory,
            IActiveDirectoryContext directory,
            AppAuthenticationStateProvider appAuthenticationStateProvider)
        {
            var permissionApplicator = new PermissionApplicator(userStateService, appDatabaseFactory, directory);
            var appUser = new ApplicationUserState(appDatabaseFactory);
            await permissionApplicator.LoadPermissions(appUser, entry as IADUser);
            appUser.User = await appAuthenticationStateProvider.CreateDirectoryPrincipal(appUser,entry as IADUser);
            appUser.GetUserSettingFromDB();
            return appUser;
        }
    }
}
