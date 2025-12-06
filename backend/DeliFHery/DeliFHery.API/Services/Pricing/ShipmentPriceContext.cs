using DeliFHery.API.Dto;

namespace DeliFHery.API.Services.Pricing
{
    public class ShipmentPriceContext
    {
        public CalculateShipmentPriceRequest? Request { get; }

        public int StatesCrossed { get; set; }
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow;

        public ShipmentPriceContext(CalculateShipmentPriceRequest request)
        {
            Request = request;
        }
    }
}
