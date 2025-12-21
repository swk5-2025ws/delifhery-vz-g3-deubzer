using System.Data.Common;

namespace DeliFHery.API.Database
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> CreateConnectionAsync(CancellationToken ct);
    }
}
