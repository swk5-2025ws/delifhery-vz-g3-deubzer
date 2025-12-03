using System.Data.Common;

namespace DeliFHery.API.Database
{
    public delegate T RowMapper<T>(DbDataReader reader);

    public class DatabaseService
    {
        private readonly IDbConnectionFactory  _connectionFactory;
        public DatabaseService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private static void AddParameters(DbCommand cmd, QueryParameter[]? parameters)
        {
            if (parameters == null) return;
            foreach (var parameter in parameters)
            {
                var dbp = cmd.CreateParameter();
                dbp.ParameterName = parameter.Name;
                dbp.Value = parameter.Value ?? DBNull.Value;
                cmd.Parameters.Add(dbp);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, RowMapper<T> map, CancellationToken ct = default, params QueryParameter[] parameters)
        { 
            await using var conn = await _connectionFactory.CreateConnectionAsync(ct);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            AddParameters(cmd, parameters);

            var results = new List<T>();

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                results.Add(map(reader));
            }
            return results;
        }

        public async Task<int> ExecuteInsertIntAsync(
            string sql,
            CancellationToken ct = default,
            params QueryParameter[] parameters)
        {
            await using var conn = await _connectionFactory.CreateConnectionAsync(ct);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            AddParameters(cmd, parameters);

            var obj = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(obj);
        }

        public async Task<Guid> ExecuteInsertGuidAsync(
    string sql,
    CancellationToken ct = default,
    params QueryParameter[] parameters)
        {
            await using var conn = await _connectionFactory.CreateConnectionAsync(ct);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            AddParameters(cmd, parameters);

            var obj = await cmd.ExecuteScalarAsync(ct);

            if (obj == null || obj == DBNull.Value)
                throw new InvalidOperationException("Insert did not return a GUID.");

            return (Guid)obj;
        }

    }
}
