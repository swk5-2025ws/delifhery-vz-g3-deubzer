using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Repo
{
    public class CarrierRepo : ICarrierRepo
    {
        private readonly DatabaseService _db;
        public CarrierRepo(DatabaseService db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(Carrier carrier, CancellationToken ct)
        {
            const string sql = @"
                            INSERT INTO [dbo].[Carrier] (
                            api_key,
                            name,
                            is_active)
                            VALUES (
                            @apiKey,
                            @name,
                            @isActive);
                            
                            SELECT SCOPE_IDENTITY();";

            return await _db.ExecuteInsertIntAsync(sql, ct,
                new QueryParameter("apiKey", carrier.apiKey),
                new QueryParameter("name", carrier.name),
                new QueryParameter("isActive", carrier.isActive));
        }

    }
}
