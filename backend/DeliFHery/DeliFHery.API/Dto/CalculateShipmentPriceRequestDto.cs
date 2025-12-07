namespace DeliFHery.API.Dto
{
    public class CalculateShipmentPriceRequestDto
    {
        //From
        public string SenderPostalCode { get; set; } = default!;
        public string SenderCity { get; set; } = default!;
        public string SenderStreet { get; set; } = default!;
        public string SenderHouseNumber { get; set; } = default!;

        //To
        public string RecipientPostalCode { get; set; } = default!;
        public string RecipientCity { get; set; } = default!;
        public string RecipientStreet { get; set;} = default!;
        public string RecipientHouseNumber { get; set; } = default!;

        //Package

        public float WidthCm  { get; set; }
        public float HeightCm { get; set; }
        public float LengthCm { get; set; }
        public float WeightKg { get; set; }
    }
}
