namespace DeliFHery.API.Models
{
    public class ContactMethod
    {
        public int contactId {  get; set; }
        public Guid customerId { get; set; } = default!;
        public string type { get; set; } = default!;
        public string value { get; set; } = default!;
        public bool isPrimary { get; set; }
    }
}
