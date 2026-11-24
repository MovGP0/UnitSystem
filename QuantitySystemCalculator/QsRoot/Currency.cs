using ParticleLexer.StandardTokens;
using System.Globalization;

namespace QsRoot;

public static class Currency
{
    static Dictionary<string, double> _currentCurrencies;

    static Currency()
    {
        _currentCurrencies = ReadCurrenciesJson();
    }

    /// <summary>
    /// Getting the exchange rate file each day so we don't hit the service too much
    /// </summary>
    static string TodayChangeRatesFile
    {
        get
        {
            var file = string.Format("XChangeRates-{0}-{1}-{2}.json", DateTime.Today.Year, DateTime.Today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), DateTime.Today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2,'0'));
            return file;
        }
    }

    static void DownloadExchangeFile()
    {
        const string web = "https://quantitysystem.azurewebsites.net/api/ExchangeRates";

        var wc = new System.Net.WebClient();
        var xch = wc.DownloadData(web);
        File.WriteAllBytes(TodayChangeRatesFile, xch);
    }

    static Dictionary<string, double> ReadCurrenciesJson()
    {
        if (!File.Exists(TodayChangeRatesFile))
        {
            // get the file from http://openexchangerates.org
            DownloadExchangeFile();
        }

        ParticleLexer.Token currenciesJson;
        using (var rr = new StreamReader(TodayChangeRatesFile))
        {
            currenciesJson = ParticleLexer.Token.ParseText(rr.ReadToEnd());
        }

        /*
         * json format is
         *
         * { "key": value,  "key": "other value", "key":{ "key":value }
         *
         * value is either string or number
         *
         */
        currenciesJson = currenciesJson.TokenizeTextStrings();
        currenciesJson = currenciesJson.MergeTokens<WordToken>();
        currenciesJson = currenciesJson.RemoveAnySpaceTokens();
        currenciesJson = currenciesJson.RemoveNewLineTokens();
        currenciesJson = currenciesJson.MergeTokens<NumberToken>();
        currenciesJson = currenciesJson.MergeSequenceTokens<MergedToken>(
            typeof(ParticleLexer.CommonTokens.TextStringToken),
            typeof(ColonToken),
            typeof(NumberToken)
        );

        Dictionary<string, double> currencies = new();

        // find rates key
        foreach (var tok in currenciesJson)
        {
            if (tok.TokenClassType == typeof(MergedToken))
            {
                currencies.Add(tok[0].TrimTokens(1, 1).TokenValue, double.Parse(tok[2].TokenValue, CultureInfo.InvariantCulture));
            }
        }

        return currencies;
    }

    public static double CurrencyConverter(string currency)
    {
        if (_currentCurrencies.TryGetValue(currency, out var result))
        {
            return 1.0 / result;
        }
        else
        {
            return double.NaN;
        }
    }

    /// <summary>
    /// updates the currency file and the conversion factors
    /// </summary>
    public static void Update()
    {
        DownloadExchangeFile();

        _currentCurrencies = ReadCurrenciesJson();
    }
}
