using Newtonsoft.Json;

namespace ADManager.Database.Models.Rules
{
    public class AutomationRuleActionFieldValue : ActiveDirectoryFieldDbSet
    {

        public string? Value { get; set; }
        [JsonIgnore]
        public AutomationRuleAction AutomationRuleAction { get; set; }
        public int AutomationRuleActionId { get; set; }
    }
}