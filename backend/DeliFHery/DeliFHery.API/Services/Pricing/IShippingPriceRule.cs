namespace DeliFHery.API.Services.Pricing
{
    public interface IShippingPriceRule
    {
        Task<decimal> ApplyAsync(ShipmentPriceContext context, decimal currentPrice);
    }
}
