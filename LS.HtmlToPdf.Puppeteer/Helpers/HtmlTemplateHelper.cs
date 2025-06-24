
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace LS.HtmlToPdf.Puppeteer.Helpers
{
    /// <summary>
    /// <summary>
    /// Provides utility methods for processing HTML templates, including placeholder replacement,
    /// HTML encoding, section loading/injection, and concatenation.
    /// </summary>
    public static class HtmlTemplateHelper
    {
        /// <summary>
        /// Ensures the input HTML string is non-empty and contains required structural elements.
        /// Throws <see cref="ArgumentException"/> if the content is invalid.
        /// </summary>
        /// <param name="html">The HTML string to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the HTML is null, whitespace, or missing critical tags like &lt;html&gt;, &lt;body&gt;, or &lt;/body&gt;.
        /// </exception>
        public static void EnsureValid(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                throw new ArgumentException("HTML content is empty.");

            if (!Regex.IsMatch(html, @"<html[^>]*>", RegexOptions.IgnoreCase))
                throw new ArgumentException("Missing <html> tag.");

            if (!Regex.IsMatch(html, @"<body[^>]*>", RegexOptions.IgnoreCase))
                throw new ArgumentException("Missing <body> tag.");

            if (!html.Contains("</body>", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Missing </body> closing tag.");
        }

        /// <summary>
        /// Replaces placeholders in the HTML string with values from the provided dictionary.
        /// </summary>
        /// <param name="html">The HTML template string containing placeholders.</param>
        /// <param name="data">A dictionary of placeholder keys and replacement values.</param>
        /// <returns>The HTML string with placeholders replaced.</returns>
        public static string ReplacePlaceholders(string html, Dictionary<string, string> data)
        {
            foreach (var (key, value) in data)
            {
                html = html.Replace(key, value ?? string.Empty);
            }
            return html;
        }

        /// <summary>
        /// Encodes special characters in a string into HTML-safe entities.
        /// </summary>
        /// <param name="input">The raw text input.</param>
        /// <returns>The HTML-encoded string.</returns>
        public static string HtmlEncode(string input)
        {
            return WebUtility.HtmlEncode(input ?? string.Empty);
        }

        /// <summary>
        /// Loads an HTML section from the specified file path.
        /// </summary>
        /// <param name="path">The file path to the HTML section.</param>
        /// <returns>The file content as a string, or empty string if not found.</returns>
        public static string LoadSection(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }

        /// <summary>
        /// Replaces a placeholder in the template with the specified HTML section.
        /// </summary>
        /// <param name="template">The full HTML template containing a placeholder.</param>
        /// <param name="placeholder">The placeholder string to be replaced.</param>
        /// <param name="htmlSection">The HTML section to inject.</param>
        /// <returns>The updated HTML string with the section injected.</returns>
        public static string InjectSection(string template, string placeholder, string htmlSection)
        {
            return template.Replace(placeholder, htmlSection ?? string.Empty);
        }

        /// <summary>
        /// Concatenates multiple HTML sections into a single HTML string with line breaks.
        /// </summary>
        /// <param name="sections">An array of HTML fragments to concatenate.</param>
        /// <returns>The concatenated HTML string.</returns>
        public static string Concat(params string[] sections)
        {
            var sb = new StringBuilder();
            foreach (var section in sections)
            {
                sb.AppendLine(section);
            }
            return sb.ToString();
        }
    }
}
