using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IShipmentPriceRepo
    {
        Task<int> CreateAsync (ShipmentPrice price, CancellationToken ct);
    }
}
