using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Repo
{
    public class ShipmentPriceRepo : IShipmentPriceRepo
    {
        private readonly DatabaseService _db;

        public ShipmentPriceRepo(DatabaseService db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(ShipmentPrice price, CancellationToken ct)
        {
            const string sql = @"
                INSERT INTO [dbo].[ShipmentPrice](
                    shipment_id,
                    amount,
                    currency,
                    calculated_at
                )
                VALUES (
                    @shipment_id,
                    @amount,
                    @currency,
                    @calculated_at
                );
                SELECT SCOPE_IDENTITY();";
            return await _db.ExecuteInsertIntAsync(sql, ct,
                    new QueryParameter("shipment_id", price.shipmentId),
                    new QueryParameter("amount",price.amount),
                    new QueryParameter("currency", price.currency),
                    new QueryParameter("calculated_at",price.calculatedAt));

        }
    }
}
