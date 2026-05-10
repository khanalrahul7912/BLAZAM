
using ADManager.Common.Data;
using ADManager.Database.Context;
using ADManager.Database.Interfaces;
using ADManager.Helpers;
using ADManager.Localization;
using BlazorTemplater;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace ADManager.EmailMessage.Email.Base
{
    public class EmailNotificationTemplateComponent : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<AppLocalization> AppLocalization { get; set; }
        [Inject]
        protected IStringLocalizer<AppHelpLocalization> AppHelpLocalization { get; set; }
        [Inject]
        protected ApplicationInfo ApplicationInfo { get; set; }

        [Parameter]
        public MarkupString EmailMessageHeader { get; set; }
        [Parameter]
        public MarkupString EmailMessageBody { get; set; }

        [Parameter]
        public string NotificationHeader { get; set; }
        public string NotificationBody { get; set; }


        [Parameter]
        public string? EntryName { get; set; }

        [Parameter]
        public string? EntryLink { get; set; }


        protected virtual bool ShouldRenderLinks
        {
            get
            {
                if (DatabaseCache.ApplicationSettings?.AppFQDN.IsNullOrEmpty() == false)
                {
                    return true;
                }
                return false;
            }
        }


        public virtual string Render() => new ComponentRenderer<EmailNotificationTemplateComponent>()
            .UseLayout<DefaultEmailLayout>()
            .AddServiceProvider(ApplicationInfo.services)
            .Set(c => c.EmailMessageHeader, EmailMessageHeader)
                .Set(c => c.EmailMessageBody, EmailMessageBody).Render();




    }
}