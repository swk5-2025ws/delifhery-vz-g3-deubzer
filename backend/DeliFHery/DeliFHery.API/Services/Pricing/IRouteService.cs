namespace DeliFHery.API.Services.Pricing
{
    public interface IRouteService
    {
        Task<int> CalculateStatesCrossedAsync(string senderPostalCode, string recipientPostalCode);
    }
}
