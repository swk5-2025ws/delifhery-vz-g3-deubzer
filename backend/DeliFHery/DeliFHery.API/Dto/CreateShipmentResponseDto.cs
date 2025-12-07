namespace DeliFHery.API.Dto
{
    public class CreateShipmentResponseDto
    {
        public string TrackingNumber { get; set; } = default!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "EUR";
        public string PaymentUrl { get; set; } = default!;
        public string? LabelImage { get; set; }

    }
}
