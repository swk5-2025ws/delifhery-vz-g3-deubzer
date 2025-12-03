namespace DeliFHery.API.Models
{
    public class Customer
    {
        public Guid customerId {get;set;}
        public string identityProviderUserId { get; set; } = default!;
        public string username { get; set; } = default!;
        public DateTime created_at { get; set; }

    }
}
