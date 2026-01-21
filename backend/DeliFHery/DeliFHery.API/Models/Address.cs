namespace DeliFHery.API.Models
{
    public class Address
    {
        public int addressId {  get; set; }
        public string name { get; set; } = default!;
        public string street { get; set; } = default!;
        public string postalCode { get; set; } = default!;
        public string city { get; set; }= default!;
    }
}
