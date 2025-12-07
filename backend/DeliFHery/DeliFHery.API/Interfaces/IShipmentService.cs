using DeliFHery.API.Dto;

namespace DeliFHery.API.Interfaces
{
    public interface IShipmentService
    {
        Task<CreateShipmentResponseDto> CreateShipmentAsync(CreateShipmentRequestDto request, 
                                                            Guid senderCustomerId,
                                                            CancellationToken ct);
    }
}
