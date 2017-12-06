using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MorningstarQuotesScraper.Services
{
    public class WebPageDownloader : IWebPageDownloader
    {
        private readonly HttpClient _httpClient;

        public WebPageDownloader()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HtmlDocument> DownloadWebPage(string url)
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            return htmlDocument;
        }
    }
}