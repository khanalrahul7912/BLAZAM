using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ADManager.Gui.UI.Settings
{
    public partial class SettingsComponents : ValidatedForm
    {
        protected object? originalSettings;

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        [Parameter]
        public EventCallback SettingsSaved { get; set; }
    }
}