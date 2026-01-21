using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;

namespace DeliFHery.API.Repo
{
    public class NotificationSubscriptionRepo : INotificationSubscriptionRepo
    {
        private readonly DatabaseService _db;
        public NotificationSubscriptionRepo(DatabaseService db)
        {
            _db = db;
        }

        public async Task<bool> ExistAsync(int shipmentId, Guid customerId, CancellationToken ct)
        {
            const string sql = @"
                        SELECT CASE WHEN EXISTS(
                        SELECT 1
                        FROM [dbo].[NotificationSubscription]
                        WHERE shipment_id = @shipmentId AND customer_id = @customerId)
                        THEN 1 ELSE 0 END;";
            var result = await _db.QueryAsync(sql,(_) => _.GetInt32(0), ct, 
                new QueryParameter("shipmentId", shipmentId),
                new QueryParameter("customerId", customerId));

            return result.FirstOrDefault() > 0;
        }

        public async Task<IReadOnlyList<Guid>> GetSubscribedCustomerIdAsync(int shipmentId, CancellationToken ct)
        {
            const string sql = @"
                            SELECT customer_id
                            FROM [dbo].[NotificationSubscription]
                            WHERE shipment_id = @shipmentId;";

            var result = await _db.QueryAsync(sql, r => r.GetGuid(0), ct,
                new QueryParameter("shipmentId", shipmentId));

            return result.ToList();
        }

        public Task SubscribeAsync(int shipmentId, Guid customerId, CancellationToken ct)
        {
            const string sql = @"
                                INSERT INTO [dbo].[NotificationSubscription] (shipment_id, customer_id)
                                VALUES (@shipmentId, @customerId);";
            return _db.ExecuteNonQueryAsync(sql, ct,
                new QueryParameter("shipmentId", shipmentId),
                new QueryParameter("customerId", customerId));
        }

        public Task UnSubscribeAsync(int shipmentId, Guid customerId, CancellationToken ct)
        {
            const string sql = @"
                            DELETE FROM [dbo].[NotificationSubscription]
                            WHERE shipment_id = @shipmentId AND customer_id = @customerId;";
            return _db.ExecuteNonQueryAsync(sql, ct,
                new QueryParameter("shipmentId", shipmentId),
                new QueryParameter("customerId", customerId));
        }
    }
}
