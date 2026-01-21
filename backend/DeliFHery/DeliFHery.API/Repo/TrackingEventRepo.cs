using DeliFHery.API.Database;
using DeliFHery.API.DBMappers;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DeliFHery.API.Repo
{
    public class TrackingEventRepo : ITrackingEventRepo
    {

        private readonly DatabaseService _db;
        private readonly TrackingEventMapper _mapper;

        public TrackingEventRepo(DatabaseService db)
        {
            _db = db;
            _mapper = new TrackingEventMapper();
        }
        public async Task<int> CreateAsync(TrackingEvent trackingEvent, CancellationToken ct = default)
        {
            const string sql = @"
                            INSERT INTO [dbo].[TrackingEvent] (
                                shipment_id,
                                status,
                                location,
                                note,
                                occurred_at
                            )
                            VALUES (
                                @shipment_id,
                                @status,
                                @location,
                                @note,
                                @occurred_at
                            );
                            SELECT SCOPE_IDENTITY();";
            return await _db.ExecuteInsertIntAsync(sql, ct,
                new QueryParameter("shipment_id", trackingEvent.shipmentId),
                new QueryParameter("status", trackingEvent.status),
                new QueryParameter("location", trackingEvent.location),
                new QueryParameter("note", trackingEvent.note),
                new QueryParameter("occurred_at", trackingEvent.occurredAt));
        }

        public async Task<IEnumerable<TrackingEvent>> GetByShipmentIdAsync(int id, CancellationToken ct = default)
        {
            const string sql = @"
                        SELECT * 
                        FROM [dbo].[TrackingEvent]
                        WHERE shipment_id = @id;";
            return await _db.QueryAsync(sql, _mapper.MapTrackingEvent, ct,
                new QueryParameter("id", id));
        }
    }
}
