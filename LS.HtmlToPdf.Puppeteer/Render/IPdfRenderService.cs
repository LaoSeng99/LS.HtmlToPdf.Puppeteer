using PuppeteerSharp;

namespace LS.HtmlToPdf.Puppeteer.Render
{
    public interface IPdfRenderService
    {
        /// <summary>
        /// Renders the provided HTML content into a PDF and returns the result as a byte array.
        /// </summary>
        /// <param name="html">The full HTML content to render. Must include &lt;html&gt;, &lt;body&gt;, and &lt;/body&gt; tags.</param>
        /// <param name="options">
        /// Optional PDF rendering options. If null, a default configuration will be used (A4, print background, no headers/footers).
        /// </param>
        /// <returns>PDF file content as a <see cref="byte[]"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the HTML is null, empty, or structurally invalid (missing &lt;html&gt; or &lt;body&gt; tags).
        /// </exception>
        Task<byte[]> RenderHtmlAsync(string html, PdfOptions? options = null);
    }
}
