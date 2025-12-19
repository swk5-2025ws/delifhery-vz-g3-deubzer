namespace DeliFHery.API.Models
{
    public sealed class EmailOptions
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public bool UseStarTls { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string FromName { get; set; } = "DeliFHery";
        public string FromEmail { get; set; } = default!;
    }
}
