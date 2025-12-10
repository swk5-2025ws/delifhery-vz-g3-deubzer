namespace DeliFHery.API.Dto
{
    public class PaymentCallbackDto
    {
        public string apiKey { get; set; } = default!;
        public string paymentId { get; set; } = default!;
        public decimal amount { get; set; }
        public string status { get; set; } = default!;
        public string? reason { get; set; }
    }
}
