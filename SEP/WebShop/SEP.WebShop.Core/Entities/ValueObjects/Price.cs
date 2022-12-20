using CSharpFunctionalExtensions;

namespace SEP.WebShop.Core.Entities.ValueObjects
{
    public class Price : ValueObject
    {
        private Price(double value)
        {
            Value = value;
        }

        private double Value;

        public static implicit operator double(Price p) => p.Value;

        public double ToDouble()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Price> Create(double price)
        {
            if (price < 0)
                return Result.Failure<Price>("Price cannot be less than 0");
            return Result.Success(new Price(price));
        }


    }
}
