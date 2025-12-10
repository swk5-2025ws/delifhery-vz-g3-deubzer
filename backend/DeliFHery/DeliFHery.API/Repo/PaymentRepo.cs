using DeliFHery.API.Database;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Repo
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly DatabaseService _db;

        public PaymentRepo(DatabaseService db)
        {
            _db = db;
        }
        public async Task<int> CreateAsync(Payment payment, CancellationToken ct)
        {
            string sql = @"
                        INSERT INTO [dbo].[Payment](
                            shipment_id,
                            external_payment_id,
                            amount,
                            currency,
                            status,
                            callback_url,
                            redirect_url,
                            created_at,
                            completed_at
                        )
                        VALUES ( 
                            @shipmentId,
                            @externalPaymentId,
                            @amount,
                            @currency,
                            @status,
                            @callbackUrl,
                            @redirectUrl,
                            @createdAt,
                            @completedAt
                        );
                        SELECT SCOPE_IDENTITY();";

            return await _db.ExecuteInsertIntAsync(sql, ct, 
                new QueryParameter("shipmentId",payment.shipmentId),
                new QueryParameter("externalPaymentId",payment.externalPaymentId),
                new QueryParameter("amount",payment.amount),
                new QueryParameter("currency",payment.currency),
                new QueryParameter("status",payment.status),
                new QueryParameter("callbackUrl",payment.callBackUrl),
                new QueryParameter("redirectUrl",payment.redirectUrl),
                new QueryParameter("createdAt",payment.createdAt),
                new QueryParameter("completedAt",payment.completedAt));
        }

        public Task<Payment?> GetByExternalIdAsync(string externalId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Payment?> GetByIdAsync(int paymentId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatusAsync(int paymentId, string status, DateTime? completedAt, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
