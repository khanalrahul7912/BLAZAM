using Microsoft.AspNetCore.Components;

namespace ADManager.Plugins
{
    public interface IPluginView
    {
        RenderFragment? View { get; }

    }
}
