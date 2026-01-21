using DeliFHery.API.Interfaces;
using DeliFHery.API.Database;
using DeliFHery.API.Models;
using DeliFHery.API.Mappers;


namespace DeliFHery.API.Repo
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly Database.DatabaseService _db;
        private readonly CustomerMapper _mapper;
        public CustomerRepo(Database.DatabaseService db)
        {
            _db = db;
            _mapper = new CustomerMapper();
        }
        

        public async Task<Guid> CreateAsync(Customer c, CancellationToken ct = default)
        {
            const string sql = @"
                INSERT INTO dbo.Customer(identity_provider_user_id, username, created_at)
                OUTPUT INSERTED.customer_id 
                VALUES (@idp, @username, GETDATE());";
            return await _db.ExecuteInsertGuidAsync(sql,ct,
                new QueryParameter("@idp",c.identityProviderUserId),
                new QueryParameter("@username",c.username   
                ));
        }
        
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken ct = default)
        {
            const string sql = @"
               SELECT customer_id,
               identity_provider_user_id,
               username,
               created_at
               FROM [dbo].[Customer];";
            return await _db.QueryAsync(sql, _mapper.MapCustomer, ct);
        }
        public async Task<Customer?> GetByIdentityProviderUserIdAsync(string identityUserId, CancellationToken ct = default)
        {
            const string sql = @"
                SELECT customer_id,
                identity_provider_user_id,
                username,
                created_at
                FROM [dbo].[Customer]
                WHERE identity_provider_user_id = @id";
            var result = await _db.QueryAsync(sql, _mapper.MapCustomer, ct,
                new QueryParameter("id", identityUserId));
            return result.FirstOrDefault();
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            const string sql = @"
               SELECT customer_id,
               identity_provider_user_id,
               username,
               created_at
               FROM[dbo].[Customer]
               WHERE customer_id = @id;";
            var result = await _db.QueryAsync(sql, _mapper.MapCustomer, ct,
                new QueryParameter("id", id));
            return result.FirstOrDefault();
        }


        public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Customer>> FindByUsernameAsync(string username, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsernameAsync(Guid id, string username, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        
    }
}
