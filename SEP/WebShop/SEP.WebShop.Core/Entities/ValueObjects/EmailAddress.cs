using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        private EmailAddress(string value)
        {
            Value = value;
        }

        private string Value;

        public static implicit operator string(EmailAddress e) => e.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<EmailAddress> Create(string value)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<EmailAddress>("Email cannot be empty");
            if (value.Length > 50)
                return Result.Failure<EmailAddress>("Email exceeded max length (50 characters)");
            if (!regex.IsMatch(value))
                return Result.Failure<EmailAddress>("Email format is not correct");
            return Result.Success(new EmailAddress(value));
        }

    }
}
