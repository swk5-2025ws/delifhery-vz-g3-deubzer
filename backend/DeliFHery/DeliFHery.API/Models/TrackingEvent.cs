namespace DeliFHery.API.Models
{
    public class TrackingEvent
    {
        public int trackingEventId { get; set; }
        public int shipmentId { get; set; }
        public string status { get; set; } = default!;
        public string location { get; set; } = default!;
        public string note { get; set; } = default!;
        public DateTime occurredAt { get; set; }
    }
}
