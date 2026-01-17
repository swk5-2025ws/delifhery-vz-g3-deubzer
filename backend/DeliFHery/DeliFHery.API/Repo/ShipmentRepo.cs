using DeliFHery.API.Database;
using DeliFHery.API.DBMappers;
using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Repo
{
    public class ShipmentRepo : IShipmentRepo
    {
        private readonly Database.DatabaseService _db;
        private readonly ShipmentMapper _shipmentMapper;
        private readonly IAddressRepo _addressRepo;

        public ShipmentRepo(Database.DatabaseService db, IAddressRepo addressRepo)
        {
            _db = db;
            _shipmentMapper = new ShipmentMapper();
            _addressRepo = addressRepo;
        }

        public async Task<int> CreateAsync(Shipment shipment, CancellationToken ct)
        {
            const string sql = @"
                        INSERT INTO [dbo].[Shipment](sender_customer_id, sender_address_id, recipient_address_id, tracking_number, weight_kg, 
                        height_cm,width_cm, length_cm,current_status, created_at)
                        VALUES (@sender_customer_id, @sender_address_id, @recipient_address_id, @tracking_number, @weight_kg,
                        @height_cm, @width_cm, @length_cm, @current_status, @created_at);
                        SELECT SCOPE_IDENTITY();";
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

        public async Task<Shipment?> GetByIdAsync(int shipmentId, CancellationToken ct)
        {
            const string sql = @"
                        SELECT * 
                        FROM [dbo].[Shipment]
                        WHERE shipment_id = @shipmentId;";
            var result = await _db.QueryAsync(sql, _shipmentMapper.MapShipment, ct,
                new QueryParameter("shipmentId", shipmentId));
            return result.FirstOrDefault();
        }

        public async Task<Shipment?> GetShipmentByTrackingNumber(string trackingNumber, CancellationToken ct)
        {
            const string sql = @"
                        SELECT *
                        FROM [dbo].[Shipment]
                        WHERE tracking_number = @tracking_number;";
            var result = await _db.QueryAsync(sql, _shipmentMapper.MapShipment, ct,
                new QueryParameter("tracking_number", trackingNumber));
            return result.FirstOrDefault();
        }

        public async Task<Shipment?> GetShipmentByTrackingNumberAndPostalCode(string postalCode,string trackingNumber, CancellationToken ct)
        {
            const string sql = @"
                             SELECT s.* 
                             FROM dbo.Shipment s
                             INNER JOIN [dbo].[Address] recipientAddr ON s.recipient_address_id = recipientAddr.address_id
                             WHERE s.tracking_number = @tracking_number
                             AND recipientAddr.postal_code = @postal_code;";
            var result = await _db.QueryAsync(sql, _shipmentMapper.MapShipment, ct,
                new QueryParameter("tracking_number", trackingNumber),
                new QueryParameter("postal_code", postalCode));

            return result.SingleOrDefault();        
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

        public async Task<IEnumerable<MyShipmentListDto>> GetMyShipmentsList(Guid customerId, CancellationToken ct)
        {
            const string sql = @"
                        SELECT s.tracking_number,
                            a.postal_code,
                            s.current_status
                        FROM dbo.Shipment s
                        INNER JOIN dbo.Address a 
                            ON a.address_id = s.recipient_address_id
                        WHERE s.sender_customer_id = @id
                        ORDER BY s.created_at DESC;";
            return await _db.QueryAsync(sql, mapper => new MyShipmentListDto
            (
                mapper.GetString(mapper.GetOrdinal("tracking_number")),
                mapper.GetString(mapper.GetOrdinal("postal_code")),
                mapper.GetString(mapper.GetOrdinal("current_status"))
            ),
            ct,
            new QueryParameter("id", customerId)
            );
        }

        public async Task UpdateStatusAsync(int shipmentId, string newStatus, CancellationToken ct)
        {
            const string sql = @"
                        UPDATE [dbo].[Shipment]
                        SET current_status = @current_status
                        WHERE shipment_id = @shipment_id;";

            await _db.ExecuteNonQueryAsync(sql, ct,
                new QueryParameter("current_status", newStatus),
                new QueryParameter("shipment_id", shipmentId));
        }
    }
}
