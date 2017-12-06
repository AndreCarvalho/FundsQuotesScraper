using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MorningstarQuotesScraper.Services;

namespace MorningstarQuotesScraper.Tests.Integration
{
    public class FakeWebPageDownloader : IWebPageDownloader
    {
        private readonly Dictionary<string, string> _urlToTestFilenameMapping;

        public FakeWebPageDownloader(Dictionary<string, string> urlToTestFilenameMapping)
        {
            _urlToTestFilenameMapping = urlToTestFilenameMapping;
        }

        public async Task<HtmlDocument> DownloadWebPage(string url)
        {
            if (!_urlToTestFilenameMapping.TryGetValue(url, out var testFile))
                throw new ArgumentException($"Url {url} was not configured for testing");

            var htmlDocument = new HtmlDocument();

            var fileText = await File.ReadAllTextAsync($"TestPages\\{testFile}");
            htmlDocument.LoadHtml(fileText);

            return htmlDocument;
        }
    }
}