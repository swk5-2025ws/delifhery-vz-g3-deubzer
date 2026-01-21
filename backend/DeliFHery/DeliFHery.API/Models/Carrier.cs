namespace DeliFHery.API.Models
{
    public class Carrier
    {
        public int carrierId { get; set; }
        public string? apiKey { get; set; }
        public string name { get; set; } = default!;
        public bool isActive { get; set; }
    }
}
