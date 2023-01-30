using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.WebShop.Core.Entities
{
    public class Package
    {
        public Guid Id { get; set; }
        public Name Name { get; set; }
        public Currency Currency { get; set; }
        public Price Price { get; set; }

        public Package(Guid id, Name name, Currency currency, Price price)
        {
            Id = id;
            Name = name;
            Currency = currency;
            Price = price;
        }

        public static Result<Package> Create(Guid id, string name, string currency, double price)
        {
            Result<Name> nameResult = Name.Create(name);
            Result<Price> priceResult = Price.Create(price);
            Result<Currency> currencyResult = Currency.Create(currency);
            if (Result.Combine(nameResult, priceResult, currencyResult).IsFailure)
            {
                return Result.Failure<Package>("Package creating failed because of invalid parameters");
            }
            return Result.Success(new Package(id, nameResult.Value, currencyResult.Value, priceResult.Value));
        }

        public Package Update(Package package) => Create(Id, package.Name, package.Currency, package.Price).Value;
    }
}
