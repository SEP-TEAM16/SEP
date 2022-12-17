using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Lastname : ValueObject
    {
        private Lastname(string name)
        {
            Value = name;
        }

        private string Value;

        public static implicit operator string(Lastname l) => l.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Lastname> Create(string name)
        {
            Regex regex = new Regex(@"^[\p{L} ]+$");
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Lastname>("Lastname cannot be empty");
            if (name.Length > 50)
                return Result.Failure<Lastname>("Lastname exceeded max length (50 characters)");
            if (!regex.IsMatch(name))
                return Result.Failure<Lastname>("Lastname can only contains letters and white spaces");
            return Result.Success(new Lastname(name));
        }

    }
}
