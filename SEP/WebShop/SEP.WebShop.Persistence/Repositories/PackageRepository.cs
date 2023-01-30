using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.WebShop.Persistence.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public PackageRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public void Add(Package entity)
        {
            using SqlCommand command = new SqlCommand("insert into dbo.Packages(id, name, currency, price) values(@id, @name, @currency, @price)", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@name", entity.Name);
            command.Parameters.AddWithValue("@currency", entity.Currency);
            command.Parameters.AddWithValue("@price", entity.Price);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Package> FindAll()
        {
            List<Package> packages = new List<Package>();
            using SqlCommand command = new SqlCommand("select id, name, currency, price from dbo.Packages", _connection, _transaction);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    packages.Add(Package.Create(reader.GetGuid("id"), reader.GetString("name"), reader.GetString("currency"), reader.GetDouble("price")).Value);
                }
            }
            return packages;
        }

        public Maybe<Package> FindById(Guid id)
        {
            using SqlCommand command = new SqlCommand("select id, name, currency, price from dbo.Packages where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return Maybe<Package>.None;
                }
                return Package.Create(reader.GetGuid("id"), reader.GetString("name"), reader.GetString("currency"), reader.GetDouble("price")).Value;
            }
        }

        public void Remove(Guid id)
        {
            using SqlCommand command = new SqlCommand("delete from dbo.Packages where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void Update(Package entity)
        {
            using SqlCommand command = new SqlCommand("update dbo.Packages set name = @name, price = @price, currency = @currency where id = @id", _connection, _transaction);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@name", entity.Name);
            command.Parameters.AddWithValue("@currency", entity.Currency);
            command.Parameters.AddWithValue("@price", entity.Price);
            command.ExecuteNonQuery();
        }
    }
}
