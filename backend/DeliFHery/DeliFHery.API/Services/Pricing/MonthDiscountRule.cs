namespace DeliFHery.API.Services.Pricing
{
    public class MonthDiscountRule : IShippingPriceRule
    {
        private const decimal DISCOUNT_FACTOR = 0.9m;

        public Task<decimal> ApplyAsync(ShipmentPriceContext context, decimal currentPrice)
        {
            var date = context.CalculationDate;
            if (date.Month == 1 || date.Month == 6 || date.Month == 12)
            {
                return Task.FromResult(currentPrice * DISCOUNT_FACTOR);
            }
            return Task.FromResult(currentPrice);
        }
    }
}
