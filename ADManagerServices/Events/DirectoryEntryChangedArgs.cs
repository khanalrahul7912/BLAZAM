using ADManager.ActiveDirectory.Interfaces;

namespace ADManager.Services.Events
{
    public class DirectoryEntryChangedArgs : BaseEventArgs
    {
        public IDirectoryEntryAdapter Entry { get; set; }

        public List<AuditChangeLog> Changes { get; set; }

        public IDirectoryEntryAdapter? Target { get; set; }

        public IDirectoryEntryAdapter? Origin { get; set; }

        public IDirectoryEntryAdapter? OriginalEntry { get; set; }

        public ActiveDirectoryObjectType ObjectType { get => Entry.ObjectType; }


    }
}
