using kli.ThumbsFunctions.Infrastructure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

[assembly: WebJobsStartup(typeof(FunctionStartup))]

namespace kli.ThumbsFunctions.Infrastructure
{
    public class FunctionStartup : IWebJobsStartup
	{
		public void Configure(IWebJobsBuilder builder)
		{
            builder.AddExtension<HttpClientFactoryExtensionConfigProvider>();

            builder.Services.AddHttpClient();
            builder.Services.Configure<HttpClientFactoryOptions>(options => options.SuppressHandlerScope = true);
		}
	}
}
