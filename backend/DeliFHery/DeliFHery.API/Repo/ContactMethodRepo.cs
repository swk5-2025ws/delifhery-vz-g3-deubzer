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

        public async Task<int> CreateAsync(ContactMethod contactMethod, CancellationToken ct = default)
        {
            const string sql = @"
                            INSERT INTO dbo.ContactMethod(customer_id, type, value, is_verified)
                            VALUES (@customerId, @type , @value, @is_verified);
                            SELECT SCOPE_IDENTITY();";
            return await _db.ExecuteInsertIntAsync(sql, ct,
                  new QueryParameter("customerId", contactMethod.customerId),
                  new QueryParameter("type", contactMethod.type),
                  new QueryParameter("value", contactMethod.value),
                  new QueryParameter("is_verified", contactMethod.isPrimary));
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
                                AND [is_verified] = 1;";
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
                            is_verified
                            FROM [dbo].[ContactMethod]
                            WHERE customer_id = @id";
            return await _db.QueryAsync(sql, _mapper.MapContactMethod, ct,
                  new QueryParameter("id", customerId));
        }

        public Task<bool> UpdateAsync(ContactMethod contactMethod, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
