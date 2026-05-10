using ADManager.Global.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADManager.Global
{
    public static class ApplicationEvents
    {

        /// <summary>
        /// Called when a user logs in or out of the application
        /// </summary>
        public static AppEvent<int> LoggedOnUserCountChanged { get; set; } = new();

        /// <summary>
        /// Called when permission are changed by an admin
        /// </summary>
        public static AppEvent PermissionsChanged { get; set; } = new();

    }
}
