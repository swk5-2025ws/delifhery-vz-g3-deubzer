using DeliFHery.API.Interfaces;

namespace DeliFHery.API.Services.Pricing
{
    public class StateSurChargeRule : IShippingPriceRule
    {
        private readonly IRouteService _routeService;
        private const decimal SUR_CHARGE = 1.50m;

        public StateSurChargeRule(IRouteService routeService)
        {
            _routeService = routeService;
        }
        public async Task<decimal> ApplyAsync(ShipmentPriceContext context, decimal currentPrice)
        {
            var request = context.Request;

            if (request == null)
            {
                throw new ArgumentNullException(nameof(context.Request));
            }

            int states = await _routeService.CalculateStatesCrossedAsync(request.SenderPostalCode, request.RecipientPostalCode);

            context.StatesCrossed = states;

            var surcharge = (states - 1) * SUR_CHARGE;

            if (surcharge < 0)
            {
                surcharge = 0;
            }

            return currentPrice + surcharge;
        }
    }
}
