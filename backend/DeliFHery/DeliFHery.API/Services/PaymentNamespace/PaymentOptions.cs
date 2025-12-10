namespace DeliFHery.API.Services.PaymentNamespace
{
    public class PaymentOptions
    {
        public string ApiKey { get; set; } = default!;
        public string StartUrl { get; set; } = default!;
        public string CallbackUrl { get; set; } = default!;
        public string RedirectUrl { get; set; } = default!;
    }
}   
