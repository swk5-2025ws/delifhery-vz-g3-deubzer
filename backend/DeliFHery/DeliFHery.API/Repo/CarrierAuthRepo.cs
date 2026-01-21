using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;

namespace DeliFHery.API.Repo
{


    public class CarrierAuthRepo : ICarrierAuthRepo
    {
        private readonly DatabaseService _db;
        public CarrierAuthRepo(DatabaseService databaseService)
        {
            _db = databaseService;
        }

        public async Task<bool> IsValidAPIKeyAsync(string apiKey, CancellationToken ct)
        {
            const string sql = @"
                            SELECT TOP 1 1
                            FROM [dbo].[Carrier]
                            WHERE api_key = @apiKey AND is_active = 1;";

            var result = await _db.QueryAsync(sql, reader => reader.GetInt32(0), ct,
                new QueryParameter("apiKey", apiKey));

            return result.Any();
            
        }
    }
}
