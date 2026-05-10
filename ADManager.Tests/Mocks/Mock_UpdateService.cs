using ADManager.FileSystem;
using ADManager.Global.Data;
using ADManager.Update;
using ADManager.Update.Services;
using System.Diagnostics;

namespace ADManager.Tests.Mocks
{
    internal class Mock_UpdateService : UpdateService
    {
        public Mock_UpdateService() : base(new()
        {
            ApplicationRoot = new SystemDirectory("C:\\temp"),
            RunningProcess = Process.GetCurrentProcess(),
            RunningVersion = new ApplicationVersion("0.0.1"),
            TempDirectory = new SystemDirectory("C:\\temp")
        }, null)
        {

            SelectedBranch = ApplicationReleaseBranches.Net8ReleasePrefix;
        }
    }
}
