namespace DeliFHery.API.Dto
{
    public class PaymentStartRequestDto
    {
        public string paymentId { get; set; } = default!;
        public decimal amount { get; set; }
        public string currency { get; set; } = "EUR";
        public string callbackUrl {  get; set; } = default!;
        public string? redirectUrl { get; set; } = default!;
    }


}
