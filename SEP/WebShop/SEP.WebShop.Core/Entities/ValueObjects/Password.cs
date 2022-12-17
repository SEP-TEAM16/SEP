using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Password : ValueObject
    {
        private Password(string value)
        {
            Value = value;
        }

        private string Value;

        public static implicit operator string(Password p) => p.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Password> Create(string value)
        {
            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasMinimum8Chars = new Regex(@".{8,}");

            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<Password>("Password cannot be empty");
            if (value.Length > 50)
                return Result.Failure<Password>("Password exceeded max length (50 characters)");
            if (!hasNumber.IsMatch(value) || !hasUpperChar.IsMatch(value) || !hasMinimum8Chars.IsMatch(value))
                return Result.Failure<Password>("Password must contains at least 1 number, 1 upper case character and needs to be longer than 8 characters");
            return Result.Success(new Password(value));
        }

    }
}
