using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Repositories;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SEP.WebShop.Persistence.Repositories
{
    public class SubscriptionOptionRepository : ISubscriptionOptionRepository
    {
        private SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public SubscriptionOptionRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<SubscriptionOption> FindAll()
        {
            List<SubscriptionOption> subscriptionOptions = new List<SubscriptionOption>();
            using SqlCommand command = new SqlCommand("select id, subscriptionType, name, price, currency from dbo.SubscriptionOptions", _connection, _transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    subscriptionOptions.Add(SubscriptionOption.Create(reader.GetGuid("id"), (SubscriptionType)reader.GetInt32("subscriptionType"), reader.GetString("name"), reader.GetDouble("price"), reader.GetString("currency")).Value);
                }
            }
            return subscriptionOptions;
        }

        public Maybe<SubscriptionOption> FindById(Guid id)
        {
            using SqlCommand command = new SqlCommand("select id, subscriptionType, name, price, currency from dbo.SubscriptionOptions where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return Maybe<SubscriptionOption>.None;
                }
                return SubscriptionOption.Create(reader.GetGuid("id"), (SubscriptionType)reader.GetInt32("subscriptionType"), reader.GetString("name"), reader.GetDouble("price"), reader.GetString("currency")).Value;
            }
        }

        public void Update(SubscriptionOption entity)
        {
            using SqlCommand command = new SqlCommand("update dbo.SubscriptionOptions set subscriptionType = @subscriptionType, name = @name, price = @price, currency = @currency where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@subscriptionType", entity.SubscriptionType);
            command.Parameters.AddWithValue("@name", entity.Name.ToString());
            command.Parameters.AddWithValue("@price", entity.Price.ToDouble());
            command.Parameters.AddWithValue("@currency", entity.Currency.ToString());
            command.ExecuteNonQuery();
        }

        public void Remove(Guid id)
        {
            using SqlCommand command = new SqlCommand("delete from dbo.SubscriptionOptions where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            using SqlCommand subscriptionCommand = new SqlCommand("delete from dbo.Subscriptions where subscriptionOptionId = @subscriptionOptionId", _connection, _transaction);
            subscriptionCommand.Parameters.AddWithValue("@subscriptionOptionId", id);
            subscriptionCommand.ExecuteNonQuery();
        }

        public void Add(SubscriptionOption entity)
        {
            using SqlCommand command = new SqlCommand("insert into dbo.SubscriptionOptions(id, subscriptionType, name, price, currency) values(@id, @subscriptionType, @name, @price, @currency)", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@subscriptionType", entity.SubscriptionType);
            command.Parameters.AddWithValue("@name", entity.Name.ToString());
            command.Parameters.AddWithValue("@price", entity.Price.ToDouble());
            command.Parameters.AddWithValue("@currency", entity.Currency.ToString());
            command.ExecuteNonQuery();
        }
    }
}
