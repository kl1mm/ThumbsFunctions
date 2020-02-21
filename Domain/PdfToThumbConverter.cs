using kli.ThumbsFunctions.GhostScript;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace kli.ThumbsFunctions
{
    internal class PdfToThumbConverter
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        
        public PdfToThumbConverter(ILogger logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        internal async Task<Stream> Convert(ConversionArguments arguments)
        {
            var pdfBytes = await this.DownloadPdfAsync(arguments.Url);
            return await this.GetThumbnailPng(pdfBytes, arguments.Size);
        }

        private async Task<byte[]> DownloadPdfAsync(Uri url)
        {
            return await this.httpClient.GetByteArrayAsync(url);
        }

        private async Task<Stream> GetThumbnailPng(byte[] pdfBytes, int size)
        {
			try
			{
				var inputFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                var outputFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                await File.WriteAllBytesAsync(inputFile, pdfBytes);
                GhostscriptWrapper.GenerateThumbnail(inputFile, outputFile);

                var resultStream = new MemoryStream();
                using (var image = Image.Load(outputFile))
                {
                    image.Mutate(ctx => ctx.Resize(size, size * image.Height / image.Width));
                    image.SaveAsJpeg(resultStream);
                }
            
                File.Delete(inputFile);
                File.Delete(outputFile);

                resultStream.Seek(0, SeekOrigin.Begin);
                return resultStream;
			}
			catch (Exception ex)
			{
				this.logger.LogWarning(ex.Message);
				return Stream.Null;
			}
		}
    }
}
