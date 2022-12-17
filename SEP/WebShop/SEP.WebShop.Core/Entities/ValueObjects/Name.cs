using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Name : ValueObject
    {
        private Name(string name)
        {
            Value = name;
        }

        private string Value;

        public static implicit operator string(Name f) => f.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Name> Create(string name)
        {
            Regex regex = new Regex(@"^[\p{L} ]+$");
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Name>("Name cannot be empty");
            if (name.Length > 50)
                return Result.Failure<Name>("Name exceeded max length (50 characters)");
            if (!regex.IsMatch(name))
                return Result.Failure<Name>("Name can only contains letters and white spaces");
            return Result.Success(new Name(name));
        }

    }
}
