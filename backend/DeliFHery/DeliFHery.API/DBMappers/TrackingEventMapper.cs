using DeliFHery.API.Models;
using System.Data.Common;

namespace DeliFHery.API.DBMappers
{
    public class TrackingEventMapper
    {
        public TrackingEvent MapTrackingEvent(DbDataReader reader)
        {
            return new TrackingEvent
            {
                trackingEventId = reader.GetInt32(reader.GetOrdinal("tracking_event_id")),
                shipmentId = reader.GetInt32(reader.GetOrdinal("shipment_id")),
                status = reader.GetString(reader.GetOrdinal("status")),
                location = reader.GetString(reader.GetOrdinal("location")),
                note = reader.GetString(reader.GetOrdinal("note")),
                occurredAt = reader.GetDateTime(reader.GetOrdinal("occurred_at"))
            };
        }
    }
}
