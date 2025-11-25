using System.Data.Common;
using System.Data.SqlClient;

namespace DeliFHery.API.Database
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString) => _connectionString = connectionString;

        public async Task<DbConnection> CreateConnectionAsync(CancellationToken ct = default)
        {
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(ct);
            return conn;
        }
    }
}
