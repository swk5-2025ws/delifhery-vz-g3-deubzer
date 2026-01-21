using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface ITrackingEventRepo
    {
        public Task<int> CreateAsync(TrackingEvent trackingEvent, CancellationToken ct);
        public Task<IEnumerable<TrackingEvent>> GetByShipmentIdAsync(int id, CancellationToken ct);
    }
}
