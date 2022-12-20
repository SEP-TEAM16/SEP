using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace SEP.WebShop.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public PaymentRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public void Add(Payment entity)
        {
            using SqlCommand command = new SqlCommand("insert into dbo.Payments(id, itemName, price, currency, subscriptionId, paymentStatus) values(@id, @itemName, @price, @currency, @subscriptionId, @paymentStatus)", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@itemName", entity.ItemName.ToString());
            command.Parameters.AddWithValue("@price", entity.Price.ToDouble());
            command.Parameters.AddWithValue("@currency", entity.Currency.ToString());
            command.Parameters.AddWithValue("@subscriptionId", entity.SubscriptionId);
            command.Parameters.AddWithValue("@paymentStatus", entity.PaymentStatus);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Payment> FindAll()
        {
            List<Payment> payments = new List<Payment>();
            using SqlCommand command = new SqlCommand("select id, itemName, price, currency, subscriptionId, paymentStatus from dbo.Payments", _connection, _transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    payments.Add(Payment.Create(reader.GetGuid("id"), reader.GetString("itemName"), reader.GetDouble("price"), reader.GetString("currency"), reader.GetGuid("subscriptionId"), (PaymentStatus)reader.GetInt32("paymentStatus")).Value);
                }
            }
            return payments;
        }

        public Maybe<Payment> FindById(Guid id)
        {
            using SqlCommand command = new SqlCommand("select id, itemName, price, currency, subscriptionId, paymentStatus from dbo.Payments where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return Maybe<Payment>.None;
                }
                return Payment.Create(reader.GetGuid("id"), reader.GetString("itemName"), reader.GetDouble("price"), reader.GetString("currency"), reader.GetGuid("subscriptionId"), (PaymentStatus)reader.GetInt32("paymentStatus")).Value;
            }
        }

        public void Remove(Guid id)
        {
            using SqlCommand command = new SqlCommand("delete from dbo.Payments where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void Update(Payment entity)
        {
            using SqlCommand command = new SqlCommand("update dbo.Payments set itemName = @itemName, price = @price, currency = @currency, subscriptionId = @subscriptionId, paymentStatus = @paymentStatus where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@itemName", entity.ItemName.ToString());
            command.Parameters.AddWithValue("@price", entity.Price.ToDouble());
            command.Parameters.AddWithValue("@currency", entity.Currency.ToString());
            command.Parameters.AddWithValue("@subscriptionId", entity.SubscriptionId);
            command.Parameters.AddWithValue("@paymentStatus", entity.PaymentStatus);
            command.ExecuteNonQuery();
        }
    }
}
