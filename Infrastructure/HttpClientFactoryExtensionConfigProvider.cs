using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System.Net.Http;

namespace kli.ThumbsFunctions.Infrastructure
{
    [Extension("HttpClientFactory")]
    internal class HttpClientFactoryExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientFactoryExtensionConfigProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            var bindingAttributeBindingRule = context.AddBindingRule<HttpClientFactoryAttribute>();
            bindingAttributeBindingRule.BindToInput<HttpClient>((httpClientFactoryAttribute) =>
            {
                return _httpClientFactory.CreateClient();
            });
        }
    }
}
