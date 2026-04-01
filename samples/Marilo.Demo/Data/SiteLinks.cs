namespace Marilo.Demo.Data;

public class SiteLinks
{
    public string DocsBaseUrl { get; set; } = "http://localhost:8081";
    public string DemoBaseUrl { get; set; } = "https://localhost:5301";

    // Docs site routes
    public string GettingStarted => $"{DocsBaseUrl}/articles/getting-started/overview.html";
    public string Theming => $"{DocsBaseUrl}/articles/theming/overview.html";
    public string ApiReference => $"{DocsBaseUrl}/api/Marilo.Core.Base.html";
    public string ApiPage(string relativePath) => $"{DocsBaseUrl}{relativePath}";

    // Demo site routes
    public string Components => $"{DemoBaseUrl}/components";
}
