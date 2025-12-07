using DeliFHery.API.Database;
using DeliFHery.API.DBMappers;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Repo
{
    public class ShipmentRepo : IShipmentRepo
    {
        private readonly Database.DatabaseService _db;
        private readonly ShipmentMapper _shipmentMapper;

        public ShipmentRepo(Database.DatabaseService db)
        {
            _db = db;
            _shipmentMapper = new ShipmentMapper();
        }

        public async Task<int> CreateAsync(Shipment shipment, CancellationToken ct)
        {
            const string sql = @"
                        INSERT INTO [dbo].[Shipment](sender_customer_id, sender_address_id, recipient_address_id, tracking_number, weight_kg, 
                        height_cm,width_cm, length_cm,current_status, created_at)
                        VALUES (@sender_customer_id, @sender_address_id, @recipient_address_id, @tracking_number, @weight_kg,
                        @height_cm, @width_cm, @length_cm, @current_status, @created_at)";
            return await _db.ExecuteInsertIntAsync(sql, ct,
                new QueryParameter("sender_customer_id", shipment.senderCustomerId),
                new QueryParameter("sender_address_id", shipment.senderAddressId),
                new QueryParameter("recipient_address_id", shipment.recipientAddressId),
                new QueryParameter("tracking_number", shipment.trackingNumber),
                new QueryParameter("weight_kg", shipment.weightKg),
                new QueryParameter("height_cm", shipment.heightCm),
                new QueryParameter("width_cm", shipment.widthCm),
                new QueryParameter("length_cm", shipment.lengthCm),
                new QueryParameter("current_status", shipment.currentStatus),
                new QueryParameter("created_at", shipment.createdAt));

        }

        public async Task<Shipment?> GetShipmentByTrackingNumber(string trackingNumber, CancellationToken ct)
        {
            const string sql = @"
                             SELECT * 
                             FROM dbo.Shipment
                             WHERE tracking_number = @tracking_number";
            var result = await _db.QueryAsync(sql, _shipmentMapper.MapShipment, ct,
                new QueryParameter("tracking_number", trackingNumber));
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Shipment>> GetShipmentsForCustomer(Guid customerId, CancellationToken ct)
        {
            const string sql = @"
                            SELECT *
                            FROM [dbo].[Shipment]
                            WHERE sender_customer_id = @id";
            return await _db.QueryAsync(sql, _shipmentMapper.MapShipment, ct,
                  new QueryParameter("id", customerId));
        }
    }
}
