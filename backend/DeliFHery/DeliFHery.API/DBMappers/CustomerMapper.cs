using DeliFHery.API.Models;
using System.Data.Common;

namespace DeliFHery.API.Mappers
{
    public class CustomerMapper
    {
        public Customer MapCustomer(DbDataReader reader)
        {
            return new Customer
            {
                customerId = reader.GetGuid(reader.GetOrdinal("customer_id")),
                identityProviderUserId = reader.GetString(reader.GetOrdinal("identity_provider_user_id")),
                username = reader.GetString(reader.GetOrdinal("username")),
                created_at = reader.GetDateTime(reader.GetOrdinal("created_at"))
            };
        }
    }
}
