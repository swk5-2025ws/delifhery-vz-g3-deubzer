using DeliFHery.API.Dto;

namespace DeliFHery.API.Services.Pricing
{
    public class ShipmentPriceContext
    {
        public CalculateShipmentPriceRequestDto? Request { get; }

        public int StatesCrossed { get; set; }
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow;

        public ShipmentPriceContext(CalculateShipmentPriceRequestDto request)
        {
            Request = request;
        }
    }
}
