using DeliFHery.API.Interfaces;
using System.Text;

namespace DeliFHery.API.Services
{
    public class LabelGenerator : ILabelGenerator
    {
        public Task<string> GenerateLabelAsync(string trackingNumber, string recipientName, string recipientStreet, string recipientHouseNumber, string recipientPostalCode, string recipientCity, CancellationToken ct)
        {
            var labelText =
                $"TRACKING: {trackingNumber}\n" +
                $"TO: {recipientName}\n" +
                $"{recipientStreet} {recipientHouseNumber}\n" +
                $"{recipientPostalCode} {recipientCity}";

            var bytes = Encoding.UTF8.GetBytes(labelText);
            var base64 = Convert.ToBase64String(bytes);

            return Task.FromResult(base64);
        }
    }
}
