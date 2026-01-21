namespace DeliFHery.API.Dto
{
    public class CreateShipmentResponseDto
    {
        public string trackingNumber { get; set; } = default!;
        public decimal price { get; set; }
        public string currency { get; set; } = "EUR";
        public string paymentUrl { get; set; } = default!;
        public string? labelImage { get; set; }
        public decimal basePrice { get; set; }
        public decimal bundeslandSurcharge { get; set; }
        public decimal seasonalDiscount { get; set; }
    }
}
