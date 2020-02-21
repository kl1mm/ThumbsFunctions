using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Net.Http;
using kli.ThumbsFunctions.Infrastructure;

namespace kli.ThumbsFunctions
{
    public static class PdfToThumbnailFunction
	{
		[FunctionName("PdfToThumbnail")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest request,
            [HttpClientFactory]HttpClient httpClient,
            ILogger logger)
		{
			string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
			var arguments = JsonConvert.DeserializeObject<ConversionArguments>(requestBody);

            var jpegStream = await new PdfToThumbConverter(logger, httpClient).Convert(arguments);
			return new FileStreamResult(jpegStream, MediaTypeNames.Image.Jpeg);
		}
	}
}
