using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MorningstarQuotesScraper.Services;

namespace MorningstarQuotesScraper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.TryAddSingleton<IWebPageDownloader, WebPageDownloader>();

            services.AddSingleton<IScrapeSettings>(s =>
            {
                var scrapeSettings = new ScrapeSettings(new CultureInfo("es-ES"),
                    "http://www.morningstar.es/es/{0}/snapshot/snapshot.aspx?id={1}");
                return scrapeSettings;
            });

            services.AddTransient<IQuoteScraper, EtfQuoteScraper>();
            services.AddTransient<IQuoteScraper, MutualFundQuoteScraper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}