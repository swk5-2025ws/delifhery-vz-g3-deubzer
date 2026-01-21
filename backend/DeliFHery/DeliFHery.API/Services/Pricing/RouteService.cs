using DeliFHery.API.Interfaces;

namespace DeliFHery.API.Services.Pricing
{
    public class RouteService : IRouteService
    {
        public Task<int> CalculateStatesCrossedAsync(string senderPostalCode, string recipientPostalCode)
        {

            int result = 0;
            if (senderPostalCode.Length >= 2 && recipientPostalCode.Length >= 2 &&
                senderPostalCode.Substring(0, 2) != recipientPostalCode.Substring(0, 2))
            {
                result = 2;
            }
            result = 1;

            return Task.FromResult(result);
        }
    }
}
