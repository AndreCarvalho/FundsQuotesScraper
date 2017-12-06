using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MorningstarQuotesScraper.Services;

namespace MorningstarQuotesScraper.Controllers
{
    [Produces("application/json")]
    [Route("api/Quotes")]
    public class QuotesController : Controller
    {
        private readonly IEnumerable<IQuoteScraper> _quoteScrapers;
        private readonly IWebPageDownloader _webPageDownloader;
        private readonly IScrapeSettings _scrapeSettings;

        public QuotesController(IEnumerable<IQuoteScraper> quoteScrapers, IWebPageDownloader webPageDownloader, IScrapeSettings scrapeSettings)
        {
            _quoteScrapers = quoteScrapers;
            _webPageDownloader = webPageDownloader;
            _scrapeSettings = scrapeSettings;
        }

        [HttpGet("{fundType}/{morningstarId}")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 8*60*60)]
        public async Task<IActionResult> GetQuoteForFund(string fundType, string morningstarId)
        {
            if (string.IsNullOrEmpty(fundType))
                return BadRequest($"{nameof(fundType)} should not be empty");
            if (string.IsNullOrEmpty(morningstarId))
                return BadRequest($"{nameof(morningstarId)} should not be empty");

            var scraper = _quoteScrapers.SingleOrDefault(x => x.HandlesFundType(fundType));

            if (scraper == null)
                return NotFound($"No scraper found for '{fundType}' type");

            var htmlDocument = await _webPageDownloader.DownloadWebPage(_scrapeSettings.GetPageUrl(fundType, morningstarId));
            var quote = scraper.Scrape(htmlDocument, _scrapeSettings.ScrapeCultureInfo);

            return Ok(quote);
        }
    }
}