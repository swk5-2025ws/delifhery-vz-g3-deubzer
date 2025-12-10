using DeliFHery.API.Database;
using DeliFHery.API.DBMappers;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Repo
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly DatabaseService _db;
        private readonly PaymentMapper _mapper;

        public PaymentRepo(DatabaseService db)
        {
            _db = db;
            _mapper = new PaymentMapper();
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

        public async Task<Payment?> GetByExternalIdAsync(string externalId, CancellationToken ct)
        {
            const string sql = @"
                        SELECT * 
                        FROM [dbo].[Payment]
                        WHERE external_payment_id = @externalPaymentId;";
            var result = await _db.QueryAsync(sql, _mapper.MapPayment, ct,
                new QueryParameter("externalPaymentId", externalId));
            return result.FirstOrDefault();
        }

        public Task<Payment?> GetByIdAsync(int paymentId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateStatusAsync(Payment payment, CancellationToken ct)
        {
            const string sql = @"UPDATE [dbo].[Payment]
                                    SET
                                        amount = @amount,
                                        currency = @currency,
                                        status = @status,
                                        callback_url = @callback_url,
                                        redirect_url = @redirect_url,
                                        completed_at = @completed_at
                                    WHERE payment_id = @payment_id";
            await _db.ExecuteNonQueryAsync(sql, ct,
                new QueryParameter("amount", payment.amount),
                new QueryParameter("currency", payment.currency),
                new QueryParameter("status", payment.status),
                new QueryParameter("callback_url", payment.callBackUrl),
                new QueryParameter("redirect_url", payment.redirectUrl),
                new QueryParameter("completed_at", payment.completedAt),
                new QueryParameter("payment_id", payment.paymentId));
            

        }
    }
}
