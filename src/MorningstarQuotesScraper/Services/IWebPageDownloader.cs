using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MorningstarQuotesScraper.Services
{
    public interface IWebPageDownloader
    {
        Task<HtmlDocument> DownloadWebPage(string url);
    }
}