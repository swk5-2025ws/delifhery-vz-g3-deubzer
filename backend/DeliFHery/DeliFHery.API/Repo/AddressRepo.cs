using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Repo
{
    public class AddressRepo : IAddressRepo
    {
        private readonly Database.DatabaseService _db;

        public AddressRepo(Database.DatabaseService db)
        {
            _db = db;
        }
        public async Task<int> CreateAsync(Address address, CancellationToken ct = default)
        {
            const string sql = @"
                INSERT INTO [dbo].[Address](
                    name,
                    street,
                    postal_code,
                    city
                )
                VALUES (
                    @name,
                    @street,
                    @postal_code,
                    @city
                );
                SELECT SCOPE_IDENTITY();";

            return await _db.ExecuteInsertIntAsync(sql, ct,
                new QueryParameter("name", address.name),
                new QueryParameter("street", address.street),
                new QueryParameter("postal_code", address.postalCode),
                new QueryParameter("city", address.city));
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Address?> GetAddressByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Address address, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
