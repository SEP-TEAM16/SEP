using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.Factories;
using SEP.WebShop.Core.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace SEP.WebShop.Persistence.Repositories
{
    public class WebShopUserRepository : IWebShopUserRepository
    {
        private SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        private WebShopUserFactory webShopUserFactory;

        public WebShopUserRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
            webShopUserFactory = new WebShopUserFactory();
        }

        public void Add(WebShopUser entity)
        {
            using SqlCommand command = new SqlCommand(
               "insert into dbo.Users(id, username, password, emailAddress, name, city, street, userType) " +
               "values(@id, @username, @password, @emailAddress, @name, @city, @street, @userType) ", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.Username.ToString());
            command.Parameters.AddWithValue("@password", entity.Password.ToString());
            command.Parameters.AddWithValue("@emailAddress", entity.EmailAddress.ToString());
            command.Parameters.AddWithValue("@name", entity.Name.ToString());
            command.Parameters.AddWithValue("@city", entity.Address.City.ToString());
            command.Parameters.AddWithValue("@street", entity.Address.Street.ToString());
            command.Parameters.AddWithValue("@userType", entity.UserType);
            command.ExecuteNonQuery();
        }

        public IEnumerable<WebShopUser> FindAll()
        {
            List<WebShopUser> webShopUsers = new List<WebShopUser>();
            using SqlCommand command = new SqlCommand(
                "select dbo.Users.id, dbo.Users.username, dbo.Users.password, dbo.Users.emailAddress, dbo.Users.name, " +
                "dbo.Users.city, dbo.Users.street, dbo.Users.userType" +
                " from dbo.Users", _connection, _transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    WebShopUser webShopUser = webShopUserFactory.Create(
                            reader.GetGuid("id"), reader.GetString("username"), reader.GetString("password"), reader.GetString("emailAddress"), reader.GetString("name"),
                            reader.GetString("city"), reader.GetString("street"), (UserType)reader.GetInt32("userType")).Value;
                    webShopUsers.Add(webShopUser);
                }
            }
            return webShopUsers;
        }

        public Maybe<WebShopUser> FindById(Guid id)
        {
            using SqlCommand command = new SqlCommand(
                "select dbo.Users.id, dbo.Users.username, dbo.Users.password, dbo.Users.emailAddress, dbo.Users.name, " +
                "dbo.Users.city, dbo.Users.street, dbo.Users.userType" +
                " from dbo.Users where dbo.Users.id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return Maybe<WebShopUser>.None;
                }
                WebShopUser webShopUser = webShopUserFactory.Create(
                           reader.GetGuid("id"), reader.GetString("username"), reader.GetString("password"), reader.GetString("emailAddress"), reader.GetString("name"),
                           reader.GetString("city"), reader.GetString("street"), (UserType)reader.GetInt32("userType")).Value;
                return webShopUser;
            }
        }

        public Maybe<WebShopUser> FindByUsername(string username)
        {
            using SqlCommand command = new SqlCommand(
                "select dbo.Users.id, dbo.Users.username, dbo.Users.password, dbo.Users.emailAddress, dbo.Users.name, " +
                "dbo.Users.city, dbo.Users.street, dbo.Users.userType" +
                " from dbo.Users where dbo.Users.username = @username", _connection, _transaction);
            command.Parameters.AddWithValue("@username", username);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return Maybe<WebShopUser>.None;
                }
                WebShopUser webShopUser = webShopUserFactory.Create(
                           reader.GetGuid("id"), reader.GetString("username"), reader.GetString("password"), reader.GetString("emailAddress"), reader.GetString("name"),
                           reader.GetString("city"), reader.GetString("street"), (UserType)reader.GetInt32("userType")).Value;
                return webShopUser;
            }
        }

        public void Remove(Guid id)
        {
            using SqlCommand command = new SqlCommand("delete from dbo.Users where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void Update(WebShopUser entity)
        {
            using SqlCommand command = new SqlCommand(
                "update dbo.Users set username = @username, password = @password, emailAddress = @emailAddress, " +
                "name = @name, city = @city, street = @street, " +
                "userType = @userType where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.Username.ToString());
            command.Parameters.AddWithValue("@password", entity.Password.ToString());
            command.Parameters.AddWithValue("@emailAddress", entity.EmailAddress.ToString());
            command.Parameters.AddWithValue("@name", entity.Name.ToString());
            command.Parameters.AddWithValue("@city", entity.Address.City.ToString());
            command.Parameters.AddWithValue("@street", entity.Address.Street.ToString());
            command.Parameters.AddWithValue("@userType", entity.UserType);
            command.ExecuteNonQuery();
        }
    }
}
