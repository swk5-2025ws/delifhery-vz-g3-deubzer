using DeliFHery.API.Dto;

namespace DeliFHery.API.Interfaces
{
    public interface ICarrierTrackingService
    {
        public Task UpdateStatusAsync(string apiKey, TrackingStatusUpdateDto dto, CancellationToken ct);
    }
}
