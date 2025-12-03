namespace DeliFHery.API.Dto
{
    public record CustomerDto
    {
        public Guid customerId { get; set; }
        public string identityProviderUserId { get; set; } = default!;
        public string username { get; set; } = default!;
        public DateTime created_at { get; set; }
    }
}
