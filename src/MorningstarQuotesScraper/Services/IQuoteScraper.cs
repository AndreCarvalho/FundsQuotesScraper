using System.Globalization;
using HtmlAgilityPack;
using MorningstarQuotesScraper.Models;

namespace MorningstarQuotesScraper.Services
{
    public interface IQuoteScraper
    {
        bool HandlesFundType(string fundType);
        QuoteData Scrape(HtmlDocument htmlDocument, CultureInfo cultureInfo);
    }
}