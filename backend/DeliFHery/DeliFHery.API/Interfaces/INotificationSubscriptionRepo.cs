namespace DeliFHery.API.Interfaces
{
    public interface INotificationSubscriptionRepo
    {
        public Task<bool> ExistAsync(int shipmentId, Guid customerId, CancellationToken ct);
        public Task SubscribeAsync(int shipmentId, Guid customerId, CancellationToken ct);
        public Task UnSubscribeAsync(int shipmentId, Guid customerId, CancellationToken ct);
        public Task<IReadOnlyList<Guid>> GetSubscribedCustomerIdAsync(int shipmentId, CancellationToken ct);
    }
}
