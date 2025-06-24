
# LS.HtmlToPdf.Puppeteer

Simple and lightweight HTML-to-PDF rendering library powered by [PuppeteerSharp](https://github.com/hardkoded/puppeteer-sharp).  
Built for scenarios like invoice generation, receipt printing, and report rendering in `.NET`.

## Features

- Convert valid HTML into PDF (`byte[]`)
- Easy integration via `IPdfRenderService`
- Chromium auto-management
- Support for header/footer templates via PuppeteerSharp
- Helper methods for placeholder injection and section composition

---

## Installation

```bash
dotnet add package LS.HtmlToPdf.Puppeteer
```

---

## Quick Start

### 1. Register Service

```csharp
builder.Services.AddSingleton<IPdfRenderService, PuppeteerPdfRenderService>();
```

### 2. Prepare HTML and data

```csharp
var htmlTemplate = File.ReadAllText("Templates/Invoice.html");
var replacements = new Dictionary<string, string>
{
    ["{{CompanyName}}"] = "LS Tools Ltd.",
    ["{{InvoiceNumber}}"] = "INV-20250624",
    ["{{Date}}"] = "2025-06-24"
};

var filledHtml = HtmlTemplateHelper.ReplacePlaceholders(htmlTemplate, replacements);
HtmlTemplateHelper.EnsureValid(filledHtml);
```

### 3. Render PDF

```csharp
var pdfBytes = await pdfService.RenderHtmlAsync(filledHtml);
File.WriteAllBytes("invoice.pdf", pdfBytes);
```

---

## Optional: Custom Header/Footer

Use PuppeteerSharp's `PdfOptions` to inject JS-based templates:

```csharp
var options = new PdfOptions
{
    DisplayHeaderFooter = true,
    HeaderTemplate = "<div style='font-size:10px; margin-left:20px;'>Invoice - {{date}}</div>",
    FooterTemplate = "<div style='font-size:10px; margin-left:20px;'>Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>",
    MarginOptions = new MarginOptions
    {
        Top = "60px",
        Bottom = "40px"
    }
};

var pdfBytes = await pdfService.RenderHtmlAsync(filledHtml, options);
```

---

## HtmlTemplateHelper API

The `HtmlTemplateHelper` class provides a set of static utilities to help process HTML templates before rendering to PDF. These methods simplify common tasks like injecting dynamic content, validating structure, and assembling fragments.

| Method | Description | Typical Use Case |
|--------|-------------|------------------|
| `ReplacePlaceholders(string html, Dictionary<string, string> data)` | Replaces placeholders in the HTML with provided values. | Populate dynamic values like `{{CustomerName}}`, `{{Total}}`, etc. |
| `EnsureValid(string html)` | Validates the HTML structure (e.g. presence of `<html>`, `<body>`, `</body>`). | Ensure the template is suitable for rendering, catch missing root tags early. |
| `HtmlEncode(string input)` | Converts special characters to HTML entities. | Prevents injection or encoding issues when inserting raw text into HTML. |
| `LoadSection(string path)` | Loads a partial HTML file from disk. | Use for headers, footers, or reusable sections like invoice line templates. |
| `InjectSection(string template, string placeholder, string htmlSection)` | Replaces a placeholder in a full HTML template with a section of content. | Insert dynamically constructed parts into base template, e.g. items table. |
| `Concat(params string[] sections)` | Concatenates multiple HTML fragments with line breaks. | Assemble multiple blocks (e.g. item rows) into one section before injection. |

### Example

```csharp
var headerHtml = HtmlTemplateHelper.LoadSection("header.html");
var footerHtml = HtmlTemplateHelper.LoadSection("footer.html");
var fullHtml = HtmlTemplateHelper.InjectSection(baseTemplate, "{{Header}}", headerHtml);
fullHtml = HtmlTemplateHelper.InjectSection(fullHtml, "{{Footer}}", footerHtml);
```



## Example: Rendering Multi-line Invoice Items

```csharp
var itemsHtml = string.Join("\n", items.Select(item => $@"
    <div class='invoice-row'>
        <div>{item.Description}</div>
        <div style='text-align:right'>{item.Amount}</div>
    </div>"));

html = HtmlTemplateHelper.InjectSection(html, "{{InvoiceItems}}", itemsHtml);
```

---

## License

MIT License  
© 2025 Laoseng  
