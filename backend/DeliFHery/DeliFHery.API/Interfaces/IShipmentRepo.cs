using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IShipmentRepo
    {
        public Task<int> CreateAsync(Shipment shipment, CancellationToken ct);
        public Task<Shipment?> GetShipmentByTrackingNumberAndPostalCode(string postalCode,string trackingNumber, CancellationToken ct);
        public Task<IEnumerable<Shipment>> GetShipmentsForCustomer(Guid customerId, CancellationToken ct);
        public Task<Shipment?> GetByIdAsync(int shipmentId, CancellationToken ct);
        public Task<Shipment?>GetShipmentByTrackingNumber(string trackingNumber, CancellationToken ct);
        public Task UpdateStatusAsync(int shipmentId, string newStatus, CancellationToken ct);
    }
}
