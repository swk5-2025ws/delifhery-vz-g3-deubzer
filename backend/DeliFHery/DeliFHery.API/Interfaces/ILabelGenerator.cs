namespace DeliFHery.API.Interfaces
{
    public interface ILabelGenerator
    {
        Task<string> GenerateLabelAsync(string trackingNumber,
            string recipientName,
            string recipientStreet,
            string recipientPostalCode,
            string recipientCity,
            CancellationToken ct);
    }
}
