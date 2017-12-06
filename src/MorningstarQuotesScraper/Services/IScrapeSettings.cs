using System.Globalization;

namespace MorningstarQuotesScraper.Services
{
    public interface IScrapeSettings
    {
        CultureInfo ScrapeCultureInfo { get; }
        string GetPageUrl(string fundType, string morningstarId);
    }
}