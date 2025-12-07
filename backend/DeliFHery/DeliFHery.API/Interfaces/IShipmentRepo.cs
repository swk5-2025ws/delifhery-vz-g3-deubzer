using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IShipmentRepo
    {
        public Task<int> CreateAsync(Shipment shipment, CancellationToken ct);
        public Task<Shipment?> GetShipmentByTrackingNumber(string trackingNumber, CancellationToken ct);
        public Task<IEnumerable<Shipment>> GetShipmentsForCustomer(Guid customerId, CancellationToken ct);

    }
}
