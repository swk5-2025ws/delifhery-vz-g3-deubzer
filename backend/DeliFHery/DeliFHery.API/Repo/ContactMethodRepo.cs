using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Mappers;
using DeliFHery.API.Models;
using ZXing;

namespace DeliFHery.API.Repo
{
    public class ContactMethodRepo : IContactMethodRepo
    {
        private readonly Database.DatabaseService _db;
        private readonly CustomerContactMapper _mapper;

        public ContactMethodRepo(Database.DatabaseService db)
        {
            _db = db;
            _mapper = new CustomerContactMapper();
        }

        public async Task<bool> CheckPrimary(Guid customerId, CancellationToken ct)
        {
            const string sql = @"
                            SELECT COUNT(1)
                            FROM [dbo].[ContactMethod]
                            WHERE customer_id = @customerId
                            AND is_primary = 1;";

            var result = await _db.ExecuteScalarAsync<int>(sql,ct,
                new QueryParameter("customerId", customerId));
            return result > 0;
        }

        public async Task<int> CreateAsync(ContactMethod contactMethod, CancellationToken ct = default)
        {
            const string sql = @"
                            INSERT INTO [dbo].[ContactMethod] (customer_id, type, value, is_primary)
                            VALUES (@customerId, @type , @value, @is_primary);
                            SELECT SCOPE_IDENTITY();";
            return await _db.ExecuteInsertIntAsync(sql, ct,
                  new QueryParameter("customerId", contactMethod.customerId),
                  new QueryParameter("type", contactMethod.type),
                  new QueryParameter("value", contactMethod.value),
                  new QueryParameter("is_primary", contactMethod.isPrimary));
        }

        public async Task<bool> DeleteAsync(Guid customerId, int contactId, CancellationToken ct = default)
        {
            const string sql = @"
                            DELETE FROM [dbo].[ContactMethod] WHERE contact_id = @contactId 
                            AND customer_id = @customer_id";

            var result =  await _db.ExecuteNonQueryAsync(sql,ct,
                new QueryParameter("@contactId", contactId),
                new QueryParameter("@customer_id",customerId));

            return result > 0;
        }

        public async Task<string?> GetPrimaryEmailAsny(Guid customerId, CancellationToken ct)
        {
            const string sql = @"
                            SELECT TOP 1 [value]
                            FROM [dbo].[ContactMethod]
                            WHERE customer_id = @customerId
                                AND [type] = 'email'
                                AND [is_primary] = 1;";
             var result = await _db.QueryAsync(sql, r => r.GetString(0), ct,
                new QueryParameter("customerId", customerId));

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<ContactMethod>> ListForCustomerAsync(Guid customerId, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT contact_id,
                            customer_id,
                            type,
                            value,
                            is_primary
                            FROM [dbo].[ContactMethod]
                            WHERE customer_id = @id";
            return await _db.QueryAsync(sql, _mapper.MapContactMethod, ct,
                  new QueryParameter("id", customerId));
        }

        public async Task<int> ClearPrimaryAsync(Guid customerId, CancellationToken ct = default)
        {
            const string sql = @"
                          UPDATE [dbo].[ContactMethod]
                          SET is_primary = 0
                          WHERE customer_id = @customerId
                            AND is_primary = 1;";
            return await _db.ExecuteNonQueryAsync(sql, ct,
                new QueryParameter("customerId", customerId));
        }

        public async Task<bool> SetPrimaryAsync(Guid customerId, int contactId, CancellationToken ct)
        {
            await ClearPrimaryAsync(customerId, ct);

            const string sql = @"
                        UPDATE [dbo].[ContactMethod]
                        SET is_primary = 1
                        WHERE customer_id = @customerId
                          AND contact_id = @contactId;";

            var result = await _db.ExecuteNonQueryAsync(sql, ct,
                new QueryParameter("customerId", customerId),
                new QueryParameter("contactId", contactId));

            return result > 0;
        }
    }
}
