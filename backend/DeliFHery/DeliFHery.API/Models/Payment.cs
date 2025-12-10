namespace DeliFHery.API.Models
{
    public class Payment
    {
        public int paymentId { get; set; }
        public int shipmentId { get; set; }
        public string? externalPaymentId { get; set; }
        public double? amount { get; set; }
        public string? currency { get; set; }
        public string? status { get; set; }
        public string? callBackUrl { get; set; }
        public string? redirectUrl { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? completedAt { get; set; }
    }
}
