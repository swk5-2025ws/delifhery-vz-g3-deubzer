namespace DeliFHery.API.Dto
{
    public class TrackingStatusRequestDto
    {
        public string TrackingNumber { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
    }

    public class TrackingStatusEventDto
    {
        public DateTime OccurredAt { get; set; }
        public string Status { get; set; } = default!;
        public string? Location { get; set; }
        public string? Note { get; set; }
    }
    public class TrackingStatusResponseDto
    {
        public string TrackingNumber { get; set; } = default!;
        public string Sender { get; set; } = default!;
        public string Recipient { get; set; } = default!;
        public List<TrackingStatusEventDto> History { get; set; } = new();
    }
    public class TrackingStatusUpdateDto
    {
        public string TrackingNumber { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string? Zusatzinformation { get; set; }
        public string? Note { get; set; }
    }
}
