namespace DeliFHery.API.Dto
{
    public class PaymentSummaryDto
    {
        public string paymentId { get; set; } = default!;
        public string status { get; set; } = default!;
        public decimal amount { get; set; }
        public string currency { get; set; } = default!;
        public string trackingNumber { get; set; } = default!;
        public string recipientName { get; set; } = default!;
        public string recipientStreet { get; set; } = default!;
        public string recipientPostalCode { get; set; } = default!;
        public string recipientCity { get; set; } = default!;
        public string labelImage { get; set; } = default!;
    }
}
