using DeliFHery.API.Dto;

namespace DeliFHery.API.Interfaces
{
    public interface IShippingPriceCalculator
    {
        Task<CalculateShipmentPriceResponseDto> CalculatePriceAsync(CalculateShipmentPriceRequestDto request);
    }
}
