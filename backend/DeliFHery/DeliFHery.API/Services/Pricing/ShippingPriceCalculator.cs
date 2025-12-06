using DeliFHery.API.Dto;

namespace DeliFHery.API.Services.Pricing
{
    public class ShippingPriceCalculator : IShippingPriceCalculator
    {

        private readonly IEnumerable<IShippingPriceRule> _rules;

        public ShippingPriceCalculator(IEnumerable<IShippingPriceRule> rules) 
        { 
            _rules = rules;
        }
        public async Task<CalculateShipmentPriceResponse> CalculatePriceAsync(CalculateShipmentPriceRequest request)
        {
            var context = new ShipmentPriceContext(request);

            decimal price = 0m;
            decimal basePrice = 0m;
            decimal stateSurCharge = 0m;
            decimal sesionalDiscount = 0;

            foreach (var rule in _rules)
            {
                var oldPrice = price;
                price = await rule.ApplyAsync(context, price);

                switch (rule)
                {
                    case BasePriceRule:
                        basePrice += price - oldPrice;
                        break;
                    case StateSurChargeRule:
                        stateSurCharge += price - oldPrice;
                        break;
                    case MonthDiscountRule:
                        sesionalDiscount += oldPrice - price;
                        break;
                }
            }

            return new CalculateShipmentPriceResponse
            {
                TotalPrice = price,
                BasePrice = basePrice,
                BundeslandSurcharge = stateSurCharge,
                SeasonalDiscount = sesionalDiscount,
                Currency = "EUR"
            };

        }
    }
}
