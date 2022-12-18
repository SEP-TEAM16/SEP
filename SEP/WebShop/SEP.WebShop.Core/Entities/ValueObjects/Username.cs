using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Username : ValueObject
    {
        private Username(string name)
        {
            Value = name;
        }

        private string Value;

        public static implicit operator string(Username u) => u.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Username> Create(string name)
        {
            Regex regex = new Regex(@"^[\p{L}\p{N}_\-]+$");
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Username>("Username cannot be empty");
            if (name.Length > 50)
                return Result.Failure<Username>("Username exceeded max length (50 characters)");
            if (!regex.IsMatch(name))
                return Result.Failure<Username>("Username can only contains alphanumeric characters and symbols '_' and '-'");
            return Result.Success(new Username(name));
        }
    }
}
