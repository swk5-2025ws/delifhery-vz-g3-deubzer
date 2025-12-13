namespace DeliFHery.API.Interfaces
{
    public interface ICarrierAuthRepo
    {
        public Task<bool> IsValidAPIKeyAsync(string apiKey, CancellationToken ct);
    }
}
