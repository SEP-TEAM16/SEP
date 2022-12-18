using CSharpFunctionalExtensions;

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
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Name>("Name cannot be empty");
            if (name.Length > 50)
                return Result.Failure<Name>("Name exceeded max length (50 characters)");
            return Result.Success(new Name(name));
        }

    }
}
