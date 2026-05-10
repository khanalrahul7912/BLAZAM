using System.Security;

namespace ADManager.Common.Data
{
    public class WindowsImpersonationUser
    {
        public string Username { get; set; }
        public string? FQDN { get; set; }
        public SecureString Password { get; set; }
    }
}