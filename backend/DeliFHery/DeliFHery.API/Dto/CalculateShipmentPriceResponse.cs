namespace DeliFHery.API.Dto
{
    public class CalculateShipmentPriceResponse
    {
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = "EUR";

        public decimal BasePrice { get; set; }
        public decimal BundeslandSurcharge { get; set; }
        public decimal SeasonalDiscount { get; set; }
    }
}
