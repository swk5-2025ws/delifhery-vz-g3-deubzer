using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface ITrackingEventRepo
    {
        public Task<int> CreateAsync(TrackingEvent trackingEvent, CancellationToken ct);
        public Task<TrackingEvent?> GetByIdAsync(int id, CancellationToken ct);
    }
}
