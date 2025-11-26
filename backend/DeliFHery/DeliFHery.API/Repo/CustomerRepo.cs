using DeliFHery.API.Interfaces;
using System.Data.Common;
using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;
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
        

        public Task<int> CreateAsync(Customer c, CancellationToken ct = default)
        {
            const string sql = @"
                INSERT INTO dbo.Customer(identity_provider_user_id, username, created_at)
                VALUES (@idp, @username, GETDATE());
                SELECT SCOPE_IDENTITY();";
            return _db.ExecuteInsertAsync(sql,ct,
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

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
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


        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Customer>> FindByUsernameAsync(string username, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsernameAsync(int id, string username, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        
    }
}
