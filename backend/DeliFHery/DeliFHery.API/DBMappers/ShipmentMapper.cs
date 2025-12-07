using DeliFHery.API.Models;
using System.Data.Common;

namespace DeliFHery.API.DBMappers
{
    public class ShipmentMapper
    {
        public Shipment MapShipment(DbDataReader reader)
        {
            return new Shipment
            {
                shipmentId = reader.GetInt32(reader.GetOrdinal("shipment_id")),
                senderCustomerId = reader.GetGuid(reader.GetOrdinal("sernder_customer_id")),
                senderAddressId = reader.GetInt32(reader.GetOrdinal("sender_address_id")),
                recipientAddressId = reader.GetInt32(reader.GetOrdinal("recipient_address_id")),
                trackingNumber = reader.GetString(reader.GetOrdinal("tracking_number")),
                weightKg = reader.GetFloat(reader.GetOrdinal("weight_kg")),
                heightCm = reader.GetFloat(reader.GetOrdinal("height_cm")),
                widthCm = reader.GetFloat(reader.GetOrdinal("width_cm")),
                lengthCm = reader.GetFloat(reader.GetOrdinal("length_cm")),
                currentStatus = reader.GetString(reader.GetOrdinal("current_status")),
                createdAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
            };
        }
    }
}
