using DeliFHery.API.Models;
using System.Data.Common;

namespace DeliFHery.API.DBMappers
{
    public class PaymentMapper
    {
        public Payment MapPayment(DbDataReader reader)
        {
            return new Payment
            {
                paymentId = reader.GetInt32(reader.GetOrdinal("payment_id")),
                shipmentId = reader.GetInt32(reader.GetOrdinal("shipment_id")),
                externalPaymentId = reader.GetString(reader.GetOrdinal("external_payment_id")),
                amount = reader.GetDouble(reader.GetOrdinal("amount")),
                currency = reader.GetString(reader.GetOrdinal("currency")),
                status = reader.GetString(reader.GetOrdinal("status")),
                callBackUrl = reader.GetString(reader.GetOrdinal("callback_url")),
                redirectUrl = reader.GetString(reader.GetOrdinal("redirect_url")),
                createdAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                completedAt = reader.IsDBNull(reader.GetOrdinal("completed_at"))
                ? (DateTime?)null
                : reader.GetDateTime(reader.GetOrdinal("completed_at")),
            };
        }
    }
}
