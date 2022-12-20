using CSharpFunctionalExtensions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Currency : ValueObject
    {
        private Currency(string name)
        {
            Value = name;
        }

        private string Value;

        public static implicit operator string(Currency c) => c.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Currency> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Currency>("Currency cannot be empty");
            if (name.Length > 50)
                return Result.Failure<Currency>("Currency exceeded max length (50 characters)");
            return Result.Success(new Currency(name));
        }

    }
}
