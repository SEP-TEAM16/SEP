using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Firstname : ValueObject
    {
        private Firstname(string name)
        {
            Value = name;
        }

        private string Value;

        public static implicit operator string(Firstname f) => f.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Firstname> Create(string name)
        {
            Regex regex = new Regex(@"^[\p{L} ]+$");
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Firstname>("Name cannot be empty");
            if (name.Length > 50)
                return Result.Failure<Firstname>("Name exceeded max length (50 characters)");
            if (!regex.IsMatch(name))
                return Result.Failure<Firstname>("Name can only contains letters and white spaces");
            return Result.Success(new Firstname(name));
        }

    }
}
