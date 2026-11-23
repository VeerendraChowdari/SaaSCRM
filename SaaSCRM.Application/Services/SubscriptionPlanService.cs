
namespace SaaSCRM.Application.Services
{
    public class SubscriptionPlanService
    {
        private static readonly Dictionary<string, decimal> PlanPrices =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Starter"] = 499m,
                ["Pro"] = 999m,
                ["Business"] = 1999m
            };

        public decimal GetPrice(string planName)
        {
            if (string.IsNullOrWhiteSpace(planName) ||
                !PlanPrices.TryGetValue(planName.Trim(), out var price))
            {
                throw new ArgumentException(
                    "Please select a valid subscription plan.");
            }

            return price;
        }
    }
}
