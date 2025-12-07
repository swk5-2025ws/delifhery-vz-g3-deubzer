namespace DeliFHery.API.Interfaces
{
    public interface IRouteService
    {
        Task<int> CalculateStatesCrossedAsync(string senderPostalCode, string recipientPostalCode);
    }
}
