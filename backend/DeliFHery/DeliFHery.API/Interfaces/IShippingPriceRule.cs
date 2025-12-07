using DeliFHery.API.Services.Pricing;

namespace DeliFHery.API.Interfaces
{
    public interface IShippingPriceRule
    {
        Task<decimal> ApplyAsync(ShipmentPriceContext context, decimal currentPrice);
    }
}
