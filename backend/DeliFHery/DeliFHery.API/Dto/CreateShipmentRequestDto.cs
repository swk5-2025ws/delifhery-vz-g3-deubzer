namespace DeliFHery.API.Dto
{
    public class CreateShipmentRequestDto
    {
        // Sender
        public string SenderName { get; set; } = default!;
        public string SenderPostalCode { get; set; } = default!;
        public string SenderStreet { get; set; } = default!;
        public string SenderCity { get; set;} = default!;

        // Recipient
        public string RecipientName { get; set; } = default!;
        public string RecipientPostalCode { get; set; } = default!;
        public string RecipientStreet { get; set;} = default!;
        public string RecipientCity { get; set; } = default!;

        //Package
        public float WidthCm { get; set; }
        public float HeightCm { get; set; }
        public float LengthCm { get; set; }
        public float WeightKg { get; set; }


    } 

    public record MyShipmentListDto
    (
         string trackingNumber, 
         string postalCode,
         string currentStatus
    );
}
