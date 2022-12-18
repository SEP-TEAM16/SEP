using CSharpFunctionalExtensions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Address : ValueObject
    {
        private Address(City city, Street street)
        {
            City = city;
            Street = street;
        }

        public City City { get; }
        public Street Street { get; }

        public static Result<Address> Create(string city, string street)
        {
            Result<Street> streetResult = Street.Create(street);
            Result<City> cityResult = City.Create(city);
            Result result = Result.Combine(cityResult, streetResult);
            if (result.IsFailure)
            {
                return Result.Failure<Address>("Address creating failed because of invalid parameters");
            }
            return Result.Success(new Address(cityResult.Value, streetResult.Value));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return Street;
        }
    }
}
