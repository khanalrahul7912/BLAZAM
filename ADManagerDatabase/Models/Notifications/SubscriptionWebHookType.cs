namespace ADManager.Database.Models.Notifications
{
    public class SubscriptionWebHookType : AppDbSetBase
    {
        public WebHookSubscription WebHookSubscription { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
