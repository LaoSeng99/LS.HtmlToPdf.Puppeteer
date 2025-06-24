
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace LS.HtmlToPdf.Puppeteer.Render
{
    internal class PuppeteerPdfRenderService : IPdfRenderService
    {
        public async Task<byte[]> RenderHtmlAsync(string html, PdfOptions? options = null)
        {
            await new BrowserFetcher().DownloadAsync();

            using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            using var page = await browser.NewPageAsync();

            await page.SetContentAsync(html, new NavigationOptions
            {
                WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
            });

            var pdfStream = await page.PdfStreamAsync(options ?? new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "0cm",
                    Right = "0cm",
                    Bottom = "0cm",
                    Left = "0cm"
                }
            });

            using var memoryStream = new MemoryStream();
            await pdfStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
