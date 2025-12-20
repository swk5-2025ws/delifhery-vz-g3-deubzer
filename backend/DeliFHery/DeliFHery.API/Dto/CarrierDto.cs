namespace DeliFHery.API.Dto
{
    public class CarrierCreateDto
    {
        public string name { get; set; } = default!;
        public string? apiKey { get; set; }
        public bool isActive { get; set; } = false;
    }

    public class CarrierUpdateDto
    {
        public string name { get; set; } = default!;
        public string? apiKey { get; set; }
        public bool isActive { get; set; }
    }

    public class CarrierResponseDto
    {
        public int carrierId { get; set; }
        public string? apiKey { get; set; }
        public string name { get; set; } = default!;
        public bool isActive { get; set; }
    }
}
