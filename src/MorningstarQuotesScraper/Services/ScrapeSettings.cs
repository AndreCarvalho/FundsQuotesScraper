using System.Globalization;

namespace MorningstarQuotesScraper.Services
{
    public class ScrapeSettings : IScrapeSettings
    {
        private readonly string _baseUrl;

        public ScrapeSettings(CultureInfo cultureInfo, string baseUrl)
        {
            _baseUrl = baseUrl;
            ScrapeCultureInfo = cultureInfo;
        }

        public CultureInfo ScrapeCultureInfo { get; }

        public string GetPageUrl(string fundType, string morningstarId)
        {
            return string.Format(_baseUrl, fundType, morningstarId);
        }
    }
}