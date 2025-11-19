
using TradeFinanceIngestionSystem.Domain.Enums;

namespace TradeFinanceIngestionSystem.Domain.ValueObjects
{
    public class Price
    {
        public decimal Amount { get; }
        public string Currency { get; }

        private Price(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"Price cannot be negative: {nameof(amount)}");
            }

            if (!Enum.TryParse<Currency>(currency, true, out _))
            {
                throw new ArgumentException($"Invalid currency: {nameof(currency)}");
            }

            Amount = amount;
            Currency = currency;
        }

        public static Price Create(decimal amount, string currency) => new(amount, currency);
    }
}
