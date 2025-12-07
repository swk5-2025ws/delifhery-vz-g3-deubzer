using System.Reflection.Metadata.Ecma335;

namespace DeliFHery.API.Models
{
    public class Shipment
    {
        public int shipmentId { get; set; }
        public Guid senderCustomerId { get; set; }
        public int senderAddressId { get; set; }
        public int recipientAddressId { get; set; }
        public string trackingNumber { get; set; } = default!;
        public double weightKg{ get; set; }
        public double heightCm { get; set; }
        public double widthCm { get; set; }
        public double lengthCm { get; set; }
        public string currentStatus { get; set; } = default!;
        public DateTime createdAt { get; set; }
    }
}
