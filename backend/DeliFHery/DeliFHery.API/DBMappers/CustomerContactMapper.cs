using DeliFHery.API.Models;
using System.Data.Common;

namespace DeliFHery.API.Mappers
{
    public class CustomerContactMapper
    {
        public ContactMethod MapContactMethod(DbDataReader reader)
        {
            return new ContactMethod
            {
                contactId = reader.GetInt32(reader.GetOrdinal("contact_id")),
                customerId = reader.GetGuid(reader.GetOrdinal("customer_id")),
                type = reader.GetString(reader.GetOrdinal("type")),
                value = reader.GetString(reader.GetOrdinal("value")),
                isPrimary = reader.GetBoolean(reader.GetOrdinal("is_primary"))
            };
        }
    }
}
