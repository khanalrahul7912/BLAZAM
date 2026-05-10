using ADManager.ActiveDirectory.Data;
using ADManager.ActiveDirectory.Interfaces;
using ADManager.Common.Data.Services;
using ADManager.Notifications.Services;

namespace ADManager.ActiveDirectory
{
    public class ActiveDirectoryContextFactory : IActiveDirectoryContextFactory
    {

        protected ActiveDirectoryContext activeDirectoryContextSeed;


        public ActiveDirectoryContextFactory(IAppDatabaseFactory dbFactory, IEncryptionService encryptionService, INotificationPublisher notificationPublisher)
        {

            activeDirectoryContextSeed = new ActiveDirectoryContext(dbFactory, encryptionService, notificationPublisher);
        }

        public IActiveDirectoryContext CreateActiveDirectoryContext(ActiveDirectoryUserState? currentUserState = null)
        {
            var context = new ActiveDirectoryContext(activeDirectoryContextSeed)
            {
                CurrentUser = currentUserState
            };
            return context;
        }





    }
}
