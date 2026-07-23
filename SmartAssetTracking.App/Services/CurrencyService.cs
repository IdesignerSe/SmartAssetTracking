namespace SmartAssetTracking.App.Services
{
    public static class CurrencyService
    {
        public static decimal ConvertUSD(decimal usd, string currency)
        {
            return currency.ToUpper() switch
            {
                "SEK" => usd * 10.5m,
                "EUR" => usd * 0.83m,
                "TRY" => usd * 32m,
                "USD" => usd,
                _ => usd
            };
        }
    }
}