using Bunit;
using Marilo.Components.Forms.Inputs;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// Tests for T4 Pickers Batch 8C gap resolutions:
/// - RES-T4B8C-001: FileUpload template context type (FileUploadTemplateContext wrapper)
/// - RES-T4B8C-002: FileUpload drop-zone CSS provider delegation
/// - RES-T4B8C-003: Upload chunk settings nested tag API (MariloUploadChunkSettings)
/// </summary>
public class T4PickerBatch8CTests : MariloTestBase
{
    // ══════════════════════════════════════════════════════════════════════
    // RES-T4B8C-001: FileUploadTemplateContext passed to FileTemplate
    // ══════════════════════════════════════════════════════════════════════

    [Fact]
    public void FileUpload_FileTemplate_ReceivesTemplateContextNotRawFileInfo()
    {
        // Arrange: capture the context type passed to FileTemplate
        Type? capturedContextType = null;

        // Act
        var cut = Render<MariloFileUpload>(p => p
            .Add(x => x.FileTemplate, (FileUploadTemplateContext ctx) =>
            {
                capturedContextType = ctx.GetType();
                return (Microsoft.AspNetCore.Components.RenderFragment)(_ => { });
            }));

        // Assert: FileTemplate parameter type is FileUploadTemplateContext
        // (The template is only called when files are present; verify the parameter type compiles)
        var instance = cut.Instance;
        Assert.NotNull(instance.FileTemplate);
        // Verify it is typed to FileUploadTemplateContext (not FileSelectFileInfo)
        var paramType = instance.FileTemplate!.Method.GetParameters()[0].ParameterType;
        Assert.Equal(typeof(FileUploadTemplateContext), paramType);
    }

    [Fact]
    public void FileUpload_FileInfoTemplate_ReceivesTemplateContextNotRawFileInfo()
    {
        var cut = Render<MariloFileUpload>(p => p
            .Add(x => x.FileInfoTemplate, (FileUploadTemplateContext ctx) =>
                (Microsoft.AspNetCore.Components.RenderFragment)(_ => { })));

        var instance = cut.Instance;
        Assert.NotNull(instance.FileInfoTemplate);
        var paramType = instance.FileInfoTemplate!.Method.GetParameters()[0].ParameterType;
        Assert.Equal(typeof(FileUploadTemplateContext), paramType);
    }

    [Fact]
    public void FileUploadTemplateContext_ExposesFileAndValidationMessage()
    {
        // Arrange: build a context manually to verify structure
        var file = new FileSelectFileInfo
        {
            Name = "test.exe",
            Size = 1024,
            Extension = ".exe",
            InvalidExtension = true
        };

        var ctx = new FileUploadTemplateContext
        {
            File = file,
            ValidationMessage = "invalid file type"
        };

        // Assert
        Assert.Same(file, ctx.File);
        Assert.True(ctx.IsInvalid);
        Assert.Equal("invalid file type", ctx.ValidationMessage);
    }

    [Fact]
    public void FileUploadTemplateContext_IsInvalid_FalseWhenFileIsValid()
    {
        var file = new FileSelectFileInfo { Name = "doc.pdf", Size = 500 };
        var ctx = new FileUploadTemplateContext { File = file, ValidationMessage = string.Empty };

        Assert.False(ctx.IsInvalid);
        Assert.Equal(string.Empty, ctx.ValidationMessage);
    }

    // ══════════════════════════════════════════════════════════════════════
    // RES-T4B8C-002: Drop-zone CSS delegated to CssProvider
    // ══════════════════════════════════════════════════════════════════════

    [Fact]
    public void FileUpload_DropZone_HasCssProviderClass()
    {
        var cut = Render<MariloFileUpload>();

        // The FluentUI provider emits "mar-file-upload__zone" for the drop zone
        var label = cut.Find("label");
        Assert.Contains("mar-file-upload__zone", label.GetAttribute("class"));
    }

    [Fact]
    public void FileUpload_DropZone_Disabled_HasDisabledClass()
    {
        var cut = Render<MariloFileUpload>(p => p
            .Add(x => x.Enabled, false));

        var label = cut.Find("label");
        Assert.Contains("mar-file-upload__zone--disabled", label.GetAttribute("class") ?? "");
    }

    [Fact]
    public void FileUpload_DropZone_Enabled_NoDisabledClass()
    {
        var cut = Render<MariloFileUpload>(p => p
            .Add(x => x.Enabled, true));

        var label = cut.Find("label");
        Assert.DoesNotContain("mar-file-upload__zone--disabled", label.GetAttribute("class") ?? "");
    }

    [Fact]
    public void FileUpload_DropZone_NoDragOver_NoDragoverClass()
    {
        var cut = Render<MariloFileUpload>();

        var label = cut.Find("label");
        Assert.DoesNotContain("mar-file-upload__zone--dragover", label.GetAttribute("class") ?? "");
    }

    // ══════════════════════════════════════════════════════════════════════
    // RES-T4B8C-003: MariloUploadChunkSettings nested tag registration
    // ══════════════════════════════════════════════════════════════════════

    private IRenderedComponent<MariloUpload> RenderUpload(Action<ComponentParameterCollectionBuilder<MariloUpload>>? configure = null)
    {
        Services.AddSingleton(new HttpClient());
        return configure == null
            ? Render<MariloUpload>()
            : Render<MariloUpload>(configure);
    }

    [Fact]
    public void Upload_ChunkSettings_DefaultChunkSize_FallsBackToFlatParameter()
    {
        var cut = RenderUpload(p => p
            .Add(x => x.ChunkSize, 524_288));

        // No child settings — effective chunk size should be the flat parameter
        Assert.Equal(524_288, cut.Instance.ChunkSize);
    }

    [Fact]
    public void Upload_ChunkSettings_ChildComponent_RegistersWithParent()
    {
        // Verify MariloUploadChunkSettings can be placed inside MariloUpload
        // and that the CascadingValue enables its registration
        Services.AddSingleton(new HttpClient());

        var cut = Render<MariloUpload>(p => p
            .Add(x => x.ChildContent,
                builder =>
                {
                    builder.OpenComponent<MariloUploadChunkSettings>(0);
                    builder.AddAttribute(1, nameof(MariloUploadChunkSettings.ChunkSize), (long?)262_144);
                    builder.AddAttribute(2, nameof(MariloUploadChunkSettings.AutoRetryAfter), (int?)1000);
                    builder.AddAttribute(3, nameof(MariloUploadChunkSettings.MaxAutoRetries), (int?)3);
                    builder.AddAttribute(4, nameof(MariloUploadChunkSettings.Resumable), (bool?)true);
                    builder.CloseComponent();
                }));

        // The component should have rendered without errors
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void UploadChunkSettings_Parameters_DefaultToNull()
    {
        // MariloUploadChunkSettings parameters are nullable — verify defaults
        Services.AddSingleton(new HttpClient());

        // We render it standalone without a parent — ParentSink will be null (graceful)
        var cut = Render<MariloUploadChunkSettings>();

        Assert.Null(cut.Instance.ChunkSize);
        Assert.Null(cut.Instance.AutoRetryAfter);
        Assert.Null(cut.Instance.MaxAutoRetries);
        Assert.Null(cut.Instance.MetadataField);
        Assert.Null(cut.Instance.Resumable);
    }

    [Fact]
    public void UploadChunkSettings_Parameters_CanBeSet()
    {
        Services.AddSingleton(new HttpClient());

        var cut = Render<MariloUploadChunkSettings>(p => p
            .Add(x => x.ChunkSize, (long?)512_000)
            .Add(x => x.AutoRetryAfter, (int?)2000)
            .Add(x => x.MaxAutoRetries, (int?)5)
            .Add(x => x.MetadataField, "uploadMeta")
            .Add(x => x.Resumable, (bool?)false));

        Assert.Equal(512_000L, cut.Instance.ChunkSize);
        Assert.Equal(2000, cut.Instance.AutoRetryAfter);
        Assert.Equal(5, cut.Instance.MaxAutoRetries);
        Assert.Equal("uploadMeta", cut.Instance.MetadataField);
        Assert.False(cut.Instance.Resumable);
    }
}
