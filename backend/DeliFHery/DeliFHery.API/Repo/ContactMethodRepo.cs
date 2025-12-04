using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using DeliFHery.API.Database;
using DeliFHery.API.Mappers;

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

        public Task<int> CreateAsync(ContactMethod contactMethod, CancellationToken ct = default)
        {
            const string sql = @"
                            INSERT INTO dbo.ContactMethod(customer_id, type, value, is_verified)
                            VALUES (@customerId, @type , @value, @is_verified);
                            SELECT SCOPE_IDENTITY();";
            return _db.ExecuteInsertIntAsync(sql, ct,
                  new QueryParameter("customerId", contactMethod.customerId),
                  new QueryParameter("type", contactMethod.type),
                  new QueryParameter("value", contactMethod.value),
                  new QueryParameter("is_verified", contactMethod.isPrimary));
        }

        public Task<bool> DeleteAsync(int contactId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ContactMethod>> ListForCustomerAsync(Guid customerId, CancellationToken ct = default)
        {
            const string sql = @"
                            SELECT contact_id,
                            customer_id,
                            type,
                            value
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
