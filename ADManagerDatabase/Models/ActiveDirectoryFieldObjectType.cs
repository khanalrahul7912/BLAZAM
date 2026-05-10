using ADManager.Common.Data;

namespace ADManager.Database.Models
{
    public class ActiveDirectoryFieldObjectType : AppDbSetBase
    {
        public ActiveDirectoryObjectType ObjectType { get; set; }
        public int ActiveDirectoryFieldId { get; set; }
    }
}