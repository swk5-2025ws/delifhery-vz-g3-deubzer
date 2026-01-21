using DeliFHery.API.Models;
using System.Data.Common;

namespace DeliFHery.API.DBMappers
{
    public class AddressMapper
    {
        public Address MapAddress(DbDataReader reader)
        {
            return new Address
            {
                addressId = reader.GetInt32(reader.GetOrdinal("address_id")),
                name = reader.GetString(reader.GetOrdinal("name")),
                street = reader.GetString(reader.GetOrdinal("street")),
                postalCode = reader.GetString(reader.GetOrdinal("postal_code")),
                city = reader.GetString(reader.GetOrdinal("city"))
            };
        }
    }
}
