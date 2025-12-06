namespace DeliFHery.API.Services.Pricing
{
    public class RouteService : IRouteService
    {
        public Task<int> CalculateStatesCrossedAsync(string senderPostalCode, string recipientPostalCode)
        {
            // TODO: echte Logik / externe API
            // Demo: wenn erste zwei Ziffern unterschiedlich -> 2 Bundesländer, sonst 1

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
