using CSharpFunctionalExtensions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Street : ValueObject
    {
        private Street(string nameAndNumber)
        {
            Value = nameAndNumber;
        }

        private string Value;

        public static implicit operator string(Street s) => s.Value;

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Street> Create(string nameAndNumber)
        {
            if (string.IsNullOrWhiteSpace(nameAndNumber))
                return Result.Failure<Street>("Street name and number cannot be empty");
            if (nameAndNumber.Length > 50)
                return Result.Failure<Street>("Street name and number exceeded max length (50 characters)");
            return Result.Success(new Street(nameAndNumber));
        }

    }
}
