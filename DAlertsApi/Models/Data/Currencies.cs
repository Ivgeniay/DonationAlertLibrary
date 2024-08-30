

namespace DAlertsApi.Models.Data
{
    public class Currencies
    {
        public Dictionary<CurrenciesType, string> currenciesData = new()
        {
            { CurrenciesType.EUR, "Euro" },
            { CurrenciesType.USD, "US Dollar" },
            { CurrenciesType.RUB, "Russian Ruble" },
            { CurrenciesType.BYN, "Belarusian Ruble" },
            { CurrenciesType.KZT, "Tenge" },
            { CurrenciesType.UAH, "Hryvnia" },
            { CurrenciesType.BRL, "Brazilian Real" },
            { CurrenciesType.TRY, "Turkish Lira" }, 
        };
    }
    public enum CurrenciesType
    {
        EUR,
        USD,
        RUB,
        BYN,
        KZT,
        UAH,
        BRL,
        TRY,
    }
}
