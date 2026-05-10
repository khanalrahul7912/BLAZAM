using ADManager.Services.Events;
using ADManager.Session;
using ADManager.Session.Interfaces;
using Microsoft.JSInterop;
using Octokit;

namespace ADManager.Services.Audit
{
    public class ServerAuditLogger : BaseAuditLogger
    {
        public ServerAuditLogger(IAppDatabaseFactory factory) : base(factory, null)
        {
        }

        protected override void TriggerDirectoryEntryChangedEvent(object? sender, DirectoryEntryChangedArgs args)
        {
            if (args.Actor is SystemUserState || args.Actor is RulesUserState || args.Actor is ActiveDirectoryUserState)
            {
                base.TriggerDirectoryEntryChangedEvent(sender, args);
            }
        }

    }
}