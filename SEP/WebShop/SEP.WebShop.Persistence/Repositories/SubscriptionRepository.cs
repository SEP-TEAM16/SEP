using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.Factories;
using SEP.WebShop.Core.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace SEP.WebShop.Persistence.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        private WebShopUserFactory _webShopUserFactory;

        public SubscriptionRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
            _webShopUserFactory = new WebShopUserFactory();
        }

        public IEnumerable<Subscription> FindAll()
        {
            List<Subscription> subscriptions = new List<Subscription>();
            using SqlCommand command = new SqlCommand("select subs.id, subs.expirationDateTime, subs.subscriptionOptionId, subs.companyId, " + //0-3
                "subOpts.id, subOpts.subscriptionType, subOpts.name, " + //4-6
                "usr.id, usr.username, usr.password, usr.emailAddress, usr.name, usr.city, usr.street, usr.userType " + //7-14
                "from dbo.Subscriptions as subs join dbo.SubscriptionOptions as subOpts on subs.subscriptionOptionId = subOpts.id " +
                "join dbo.Users as usr on subs.companyId = usr.id", _connection, _transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    SubscriptionOption subscriptionOption = SubscriptionOption.Create(reader.GetGuid(4), (SubscriptionType)reader.GetInt32(5), reader.GetString(6)).Value;
                    Company company = (Company)_webShopUserFactory.Create(
                           reader.GetGuid(7), reader.GetString(8), reader.GetString(9), reader.GetString(10), reader.GetString(11),
                           reader.GetString(12), reader.GetString(13), (UserType)reader.GetInt32(14)).Value;
                    subscriptions.Add(Subscription.Create(reader.GetGuid(0), reader.GetDateTime(1), subscriptionOption, company).Value);
                }
            }
            return subscriptions;
        }

        public Maybe<Subscription> FindById(Guid id)
        {
            using SqlCommand command = new SqlCommand("select subs.id, subs.expirationDateTime, subs.subscriptionOptionId, subs.companyId, " + //0-3
               "subOpts.id, subOpts.subscriptionType, subOpts.name, " + //4-6
               "usr.id, usr.username, usr.password, usr.emailAddress, usr.name, usr.city, usr.street, usr.userType " + //7-14
               "from dbo.Subscriptions as subs join dbo.SubscriptionOptions as subOpts on subs.subscriptionOptionId = subOpts.id " +
               "join dbo.Users as usr on subs.companyId = usr.id where subs.id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return Maybe<Subscription>.None;
                }
                SubscriptionOption subscriptionOption = SubscriptionOption.Create(reader.GetGuid(4), (SubscriptionType)reader.GetInt32(5), reader.GetString(6)).Value;
                Company company = (Company)_webShopUserFactory.Create(
                       reader.GetGuid(7), reader.GetString(8), reader.GetString(9), reader.GetString(10), reader.GetString(11),
                       reader.GetString(12), reader.GetString(13), (UserType)reader.GetInt32(14)).Value;
                return Subscription.Create(reader.GetGuid(0), reader.GetDateTime(1), subscriptionOption, company).Value;
            }
        }

        public void Update(Subscription entity)
        {
            using SqlCommand command = new SqlCommand("update dbo.Subscriptions set expirationDateTime = @expirationDateTime, subscriptionOptionId = @subscriptionOptionId, companyId = @companyId where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@expirationDateTime", entity.ExpirationDateTime);
            command.Parameters.AddWithValue("@subscriptionOptionId", entity.SubscriptionOption.Id);
            command.Parameters.AddWithValue("@companyId", entity.Company.Id);
            command.ExecuteNonQuery();
        }

        public void Remove(Guid id)
        {
            using SqlCommand command = new SqlCommand("delete from dbo.Subscriptions where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void Add(Subscription entity)
        {
            using SqlCommand command = new SqlCommand(
                "insert into dbo.Subscriptions(id, expirationDateTime, subscriptionOptionId, companyId) " +
                "values(@id, @expirationDateTime, @subscriptionOptionId, @companyId)", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@expirationDateTime", entity.ExpirationDateTime);
            command.Parameters.AddWithValue("@subscriptionOptionId", entity.SubscriptionOption.Id);
            command.Parameters.AddWithValue("@companyId", entity.Company.Id);
            command.ExecuteNonQuery();
        }
    }
}
