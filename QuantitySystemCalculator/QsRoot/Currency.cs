using ParticleLexer.StandardTokens;
using System.Globalization;

namespace QsRoot
{
    public static class Currency
    {

        static ParticleLexer.Token _currenciesJson;
        static Dictionary<string, double> _currentCurrencies;

        static Currency()
        {
            ReadCurrenciesJson();
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
            var web = "https://quantitysystem.azurewebsites.net/api/ExchangeRates";

            var wc = new System.Net.WebClient();
            var xch = wc.DownloadData(web);
            File.WriteAllBytes(TodayChangeRatesFile, xch);
        }

        static void ReadCurrenciesJson()
        {
            if (!File.Exists(TodayChangeRatesFile))
            {
                // get the file from http://openexchangerates.org
                DownloadExchangeFile();
            }

            using (var rr = new StreamReader(TodayChangeRatesFile))
            {
                _currenciesJson = ParticleLexer.Token.ParseText(rr.ReadToEnd());
            }

            /*
             * json format is 
             * 
             * { "key": value,  "key": "other value", "key":{ "key":value }
             * 
             * value is either string or number
             * 
             */
            _currenciesJson = _currenciesJson.TokenizeTextStrings();
            _currenciesJson = _currenciesJson.MergeTokens<WordToken>();
            _currenciesJson = _currenciesJson.RemoveAnySpaceTokens();
            _currenciesJson = _currenciesJson.RemoveNewLineTokens();
            _currenciesJson = _currenciesJson.MergeTokens<NumberToken>();
            _currenciesJson = _currenciesJson.MergeSequenceTokens<MergedToken>(
                typeof(ParticleLexer.CommonTokens.TextStringToken),
                typeof(ColonToken),
                typeof(NumberToken)
                );


            _currentCurrencies = new Dictionary<string, double>();

            // find rates key
            foreach (var tok in _currenciesJson)
            {
                if (tok.TokenClassType == typeof(MergedToken))
                {
                    _currentCurrencies.Add(tok[0].TrimTokens(1, 1).TokenValue, double.Parse(tok[2].TokenValue, CultureInfo.InvariantCulture));
                }
            }
        }

        public static double CurrencyConverter(string currency)
        {
            if (_currentCurrencies.TryGetValue(currency, out var result))
                return 1.0 / result;
            else
                return double.NaN;

        }

        /// <summary>
        /// updates the currency file and the conversion factors
        /// </summary>
        public static void Update()
        {
            DownloadExchangeFile();

            ReadCurrenciesJson();
        }
    }
}
