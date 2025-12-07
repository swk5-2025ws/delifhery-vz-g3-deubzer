using DeliFHery.API.Dto;

namespace DeliFHery.API.Services.Pricing
{
    public interface IShippingPriceCalculator
    {
        Task<CalculateShipmentPriceResponseDto> CalculatePriceAsync(CalculateShipmentPriceRequestDto request);
    }
}
