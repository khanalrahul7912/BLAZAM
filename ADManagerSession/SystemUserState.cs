using System.Security.Claims;

namespace ADManager.Session
{
    public class SystemUserState : ApplicationUserState
    {
        public SystemUserState(IAppDatabaseFactory factory) : base(factory)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "AD Manager"));
            identity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, "AD Manager"));
            this.User = new System.Security.Claims.ClaimsPrincipal();
            this.User.AddIdentity(identity);
        }
    }
}
