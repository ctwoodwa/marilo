using Xunit;

namespace Marilo.Tests.Unit.Interop;

/// <summary>
/// Verifies that all expected shared JS module files exist at the correct paths.
/// </summary>
public class JsModulePathTests
{
    private static readonly string JsRoot = Path.Combine(
        FindRepoRoot(),
        "src", "Marilo.Components", "wwwroot", "js");

    [Theory]
    [InlineData("marilo-measurement.js")]
    [InlineData("marilo-observers.js")]
    [InlineData("marilo-positioning.js")]
    [InlineData("marilo-dragdrop.js")]
    [InlineData("marilo-resize.js")]
    [InlineData("marilo-clipboard-download.js")]
    [InlineData("marilo-graphics.js")]
    public void Shared_Js_Module_File_Exists(string fileName)
    {
        var path = Path.Combine(JsRoot, fileName);
        Assert.True(File.Exists(path), $"Expected JS module at {path}");
    }

    [Theory]
    [InlineData("marilo-measurement.js", "getBoundingClientRect")]
    [InlineData("marilo-measurement.js", "getViewport")]
    [InlineData("marilo-observers.js", "observeResize")]
    [InlineData("marilo-observers.js", "observeIntersection")]
    [InlineData("marilo-positioning.js", "computePosition")]
    [InlineData("marilo-dragdrop.js", "startDrag")]
    [InlineData("marilo-resize.js", "startResize")]
    [InlineData("marilo-clipboard-download.js", "writeClipboard")]
    [InlineData("marilo-clipboard-download.js", "readText")]
    [InlineData("marilo-clipboard-download.js", "download")]
    [InlineData("marilo-graphics.js", "measureText")]
    [InlineData("marilo-graphics.js", "getDevicePixelRatio")]
    public void Shared_Js_Module_Exports_Expected_Function(string fileName, string functionName)
    {
        var path = Path.Combine(JsRoot, fileName);
        var content = File.ReadAllText(path);
        Assert.True(
            content.Contains($"export function {functionName}") ||
            content.Contains($"export async function {functionName}"),
            $"Expected 'export [async] function {functionName}' in {fileName}");
    }

    [Theory]
    [InlineData("marilo-measurement.js")]
    [InlineData("marilo-observers.js")]
    [InlineData("marilo-positioning.js")]
    [InlineData("marilo-dragdrop.js")]
    [InlineData("marilo-resize.js")]
    [InlineData("marilo-clipboard-download.js")]
    [InlineData("marilo-graphics.js")]
    public void Shared_Js_Module_Has_Dispose_Export(string fileName)
    {
        var path = Path.Combine(JsRoot, fileName);
        var content = File.ReadAllText(path);
        // All modules should export a dispose function for lifecycle management
        Assert.Contains("export function dispose", content);
    }

    private static string FindRepoRoot()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null)
        {
            if (Directory.Exists(Path.Combine(dir, "src", "Marilo.Components")))
                return dir;
            dir = Directory.GetParent(dir)?.FullName;
        }
        // Fallback: assume running from repo root
        return AppContext.BaseDirectory;
    }
}
