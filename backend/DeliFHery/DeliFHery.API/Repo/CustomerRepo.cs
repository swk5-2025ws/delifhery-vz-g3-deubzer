using DeliFHery.API.Interfaces;
using System.Data.Common;
using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;


namespace DeliFHery.API.Repo
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly Database.DatabaseService _db;
        public CustomerRepo(Database.DatabaseService db) => _db = db;
        

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

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Customer>> FindByUsernameAsync(string username, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsernameAsync(int id, string username, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
