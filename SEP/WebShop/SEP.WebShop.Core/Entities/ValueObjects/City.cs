using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class City : ValueObject
    {
        private City(string name)
        {
            Value = name;
        }

        private string Value;

        public static implicit operator string(City c) => c.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<City> Create(string name)
        {
            Regex regex = new Regex(@"^[\p{L} ]+$");
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<City>("City name cannot be empty");
            if (name.Length > 50)
                return Result.Failure<City>("City name exceeded max length (50 characters)");
            if (!regex.IsMatch(name))
                return Result.Failure<City>("City name can only contains letters and white spaces");
            return Result.Success(new City(name));
        }

    }
}
