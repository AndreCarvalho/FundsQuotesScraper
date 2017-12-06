using System;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using MorningstarQuotesScraper.Models;

namespace MorningstarQuotesScraper.Services
{
    public class EtfQuoteScraper : IQuoteScraper
    {
        public bool HandlesFundType(string fundType)
        {
            return string.Equals("etf", fundType, StringComparison.InvariantCultureIgnoreCase);
        }

        public QuoteData Scrape(HtmlDocument htmlDocument, CultureInfo cultureInfo)
        {
            var statsDiv = htmlDocument.GetElementbyId("overviewQuickstatsDiv");
            var statsTableRows = statsDiv.Descendants("table").First().Descendants("tr").ToArray();

            var rowWithData = statsTableRows[1];
            var tds = rowWithData.Descendants("td").ToArray();

            var dateText = tds[0].InnerText.Substring(16);
            var valueText = tds[2].InnerText.Substring(4);

            var fundValue = float.Parse(valueText, NumberStyles.AllowDecimalPoint, cultureInfo);

            var fundQuoteData = new QuoteData
            {
                Date = dateText,
                Quote = fundValue
            };
            return fundQuoteData;
        }
    }
}