namespace DeliFHery.API.Models
{
    public class ShipmentPrice
    {
        public int priceId {  get; set; }
        public int shipmentId { get; set; }
        public double amount { get; set; }
        public string currency { get; set; } = default!;
        public DateTime calculatedAt { get; set; }

    }
}
