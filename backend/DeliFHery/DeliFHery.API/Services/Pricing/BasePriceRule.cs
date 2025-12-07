using DeliFHery.API.Interfaces;

namespace DeliFHery.API.Services.Pricing
{
    public class BasePriceRule : IShippingPriceRule
    {
        private const double MAX_WEIGHT_KG = 31;
        private const double MAX_LENGTH_CM = 120;
        private const double MAX_WIDTH_CM = 60;
        private const double MAX_HEIGHT_CM = 60;

        public Task<decimal> ApplyAsync(ShipmentPriceContext context, decimal currentPrice)
        {
            var request = context.Request;
            if (request == null)
            {
                throw new NullReferenceException("No package request");
            }

            if(request.WeightKg > MAX_WEIGHT_KG ||
                request.WidthCm > MAX_WIDTH_CM ||
                request.LengthCm > MAX_LENGTH_CM ||
                request.HeightCm > MAX_HEIGHT_CM)
            {
                throw new InvalidOperationException("Package limit reached");
            }

            decimal basePrice;
            if (request.WeightKg <= 2)
            {
                basePrice = 4.99m;
            }
            else if (request.WeightKg <= 10)
            {
                basePrice = 8.99m;
            }
            else
            {
                basePrice = 14.99m;
            }

            return Task.FromResult(currentPrice + basePrice);
        }
    }
}
