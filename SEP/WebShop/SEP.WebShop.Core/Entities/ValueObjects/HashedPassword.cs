using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Utils;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class HashedPassword : ValueObject
    {
        private HashedPassword(string value)
        {
            Value = value;
        }

        private string Value;

        public static implicit operator string(HashedPassword p) => p.Value;

        public override string ToString()
        {
            return Value;
        }

        public bool Verify(string password)
        {
            return SecurePasswordHasher.Verify(password, this.Value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<HashedPassword> Create(string value)
        {
            if (!value.StartsWith("$SEPHASH$V1"))
            {
                return Result.Success(new HashedPassword(SecurePasswordHasher.Hash(value)));
            }

            return Result.Success(new HashedPassword(value));
        }
    }
}
