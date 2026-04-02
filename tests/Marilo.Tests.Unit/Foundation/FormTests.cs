using Bunit;
using Marilo.Components.Forms.Containers;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Marilo.Tests.Unit.Foundation;

public class FormTests : MariloTestBase
{
    private class TestModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email")]
        public string? Email { get; set; }
    }

    // -------------------------------------------------------------------------
    // Criterion 1: MariloForm creates EditContext from Model
    // -------------------------------------------------------------------------

    [Fact]
    public void Form_WithModel_RendersFormElement()
    {
        var model = new TestModel();

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model));

        Assert.NotNull(cut.Find("form"));
    }

    [Fact]
    public void Form_WithModel_ExposesCurrentEditContext()
    {
        var model = new TestModel();

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model));

        var instance = cut.Instance;
        Assert.NotNull(instance.CurrentEditContext);
        Assert.Same(model, instance.CurrentEditContext!.Model);
    }

    // -------------------------------------------------------------------------
    // Criterion 2: MariloForm accepts existing EditContext
    // -------------------------------------------------------------------------

    [Fact]
    public void Form_WithExistingEditContext_UsesProvidedContext()
    {
        var model = new TestModel();
        var existingContext = new EditContext(model);

        var cut = Render<MariloForm>(p => p
            .Add(f => f.EditContext, existingContext));

        Assert.Same(existingContext, cut.Instance.CurrentEditContext);
    }

    // -------------------------------------------------------------------------
    // Criterion 3: Mutual exclusion — Model XOR EditContext
    // -------------------------------------------------------------------------

    [Fact]
    public void Form_WithBothModelAndEditContext_ThrowsInvalidOperationException()
    {
        var model = new TestModel();
        var context = new EditContext(model);

        Assert.Throws<InvalidOperationException>(() =>
            Render<MariloForm>(p => p
                .Add(f => f.Model, model)
                .Add(f => f.EditContext, context)));
    }

    // -------------------------------------------------------------------------
    // Criterion 4: Submit events
    // -------------------------------------------------------------------------

    [Fact]
    public void Form_OnSubmit_FiresOnEverySubmit()
    {
        var model = new TestModel { Name = "Alice" };
        EditContext? capturedContext = null;

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.OnSubmit, EventCallback.Factory.Create<EditContext>(
                this, ctx => capturedContext = ctx)));

        cut.Find("form").Submit();

        Assert.NotNull(capturedContext);
    }

    [Fact]
    public void Form_OnValidSubmit_FiresWhenValidAndNoOnSubmitDelegate()
    {
        var model = new TestModel { Name = "Alice" };
        EditContext? capturedContext = null;

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.OnValidSubmit, EventCallback.Factory.Create<EditContext>(
                this, ctx => capturedContext = ctx)));

        cut.Find("form").Submit();

        Assert.NotNull(capturedContext);
    }

    [Fact]
    public void Form_OnInvalidSubmit_FiresWhenInvalidAndNoOnSubmitDelegate()
    {
        // Model with Name = null fails [Required]
        var model = new TestModel();
        EditContext? capturedContext = null;

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.OnInvalidSubmit, EventCallback.Factory.Create<EditContext>(
                this, ctx => capturedContext = ctx)));

        cut.Find("form").Submit();

        Assert.NotNull(capturedContext);
    }

    [Fact]
    public void Form_OnSubmitDelegate_PreventsonValidSubmitAndOnInvalidSubmitFromFiring()
    {
        var model = new TestModel(); // invalid — Name is null
        var validSubmitFired = false;
        var invalidSubmitFired = false;
        var submitFired = false;

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.OnSubmit, EventCallback.Factory.Create<EditContext>(
                this, _ => submitFired = true))
            .Add(f => f.OnValidSubmit, EventCallback.Factory.Create<EditContext>(
                this, _ => validSubmitFired = true))
            .Add(f => f.OnInvalidSubmit, EventCallback.Factory.Create<EditContext>(
                this, _ => invalidSubmitFired = true)));

        cut.Find("form").Submit();

        Assert.True(submitFired);
        Assert.False(validSubmitFired);
        Assert.False(invalidSubmitFired);
    }

    // -------------------------------------------------------------------------
    // Criterion 5: OnUpdate fires on field change
    // -------------------------------------------------------------------------

    [Fact]
    public void Form_OnUpdate_FiresWhenFieldChanges()
    {
        var model = new TestModel();
        FormUpdateEventArgs? captured = null;

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.OnUpdate, EventCallback.Factory.Create<FormUpdateEventArgs>(
                this, args => captured = args)));

        // Notify field changed directly through the EditContext
        var editContext = cut.Instance.CurrentEditContext!;
        editContext.NotifyFieldChanged(editContext.Field(nameof(TestModel.Name)));

        Assert.NotNull(captured);
        Assert.Equal(nameof(TestModel.Name), captured!.FieldName);
        Assert.Same(model, captured.Model);
    }

    // -------------------------------------------------------------------------
    // Criterion 6: EditContext is cascaded to child components
    // -------------------------------------------------------------------------

    [Fact]
    public void Form_CascadesEditContextToChildren()
    {
        var model = new TestModel();
        EditContext? receivedContext = null;

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<CascadingEditContextCapture>(0);
                builder.AddAttribute(1, nameof(CascadingEditContextCapture.OnContextReceived),
                    EventCallback.Factory.Create<EditContext>(this, ctx => receivedContext = ctx));
                builder.CloseComponent();
            })));

        Assert.NotNull(receivedContext);
        Assert.Same(model, receivedContext!.Model);
    }

    // -------------------------------------------------------------------------
    // Criterion 7: MariloValidationMessage displays per-field errors
    // -------------------------------------------------------------------------

    [Fact]
    public void ValidationMessage_ShowsFieldError_AfterValidation()
    {
        var model = new TestModel(); // Name = null → Required fails

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloValidationMessage<string?>>(0);
                builder.AddAttribute(1, nameof(MariloValidationMessage<string?>.For),
                    (System.Linq.Expressions.Expression<Func<string?>>)(() => model.Name));
                builder.CloseComponent();
            })));

        // Trigger validation
        cut.Find("form").Submit();

        Assert.Contains("Name is required", cut.Markup);
    }

    // -------------------------------------------------------------------------
    // Criterion 8: MariloValidationSummary displays all errors
    // -------------------------------------------------------------------------

    [Fact]
    public void ValidationSummary_ShowsAllErrors_AfterValidation()
    {
        var model = new TestModel(); // Name required, Email not set (valid — null passes email)

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloValidationSummary>(0);
                builder.CloseComponent();
            })));

        cut.Find("form").Submit();

        Assert.Contains("Name is required", cut.Markup);
    }

    // -------------------------------------------------------------------------
    // Criterion 9: MariloValidationTooltip displays per-field errors
    // -------------------------------------------------------------------------

    [Fact]
    public void ValidationTooltip_ShowsFieldError_AfterValidation()
    {
        var model = new TestModel();

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloValidationTooltip<string?>>(0);
                builder.AddAttribute(1, nameof(MariloValidationTooltip<string?>.For),
                    (System.Linq.Expressions.Expression<Func<string?>>)(() => model.Name));
                builder.CloseComponent();
            })));

        cut.Find("form").Submit();

        Assert.Contains("Name is required", cut.Markup);
    }

    // -------------------------------------------------------------------------
    // Criterion 10: Validation components support Template
    // -------------------------------------------------------------------------

    [Fact]
    public void ValidationMessage_SupportsCustomTemplate()
    {
        var model = new TestModel();

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloValidationMessage<string?>>(0);
                builder.AddAttribute(1, nameof(MariloValidationMessage<string?>.For),
                    (System.Linq.Expressions.Expression<Func<string?>>)(() => model.Name));
                builder.AddAttribute(2, nameof(MariloValidationMessage<string?>.Template),
                    (RenderFragment<IEnumerable<string>>)(messages => innerBuilder =>
                    {
                        innerBuilder.OpenElement(0, "ul");
                        innerBuilder.AddAttribute(1, "class", "custom-errors");
                        foreach (var msg in messages)
                        {
                            innerBuilder.OpenElement(2, "li");
                            innerBuilder.AddContent(3, msg);
                            innerBuilder.CloseElement();
                        }
                        innerBuilder.CloseElement();
                    }));
                builder.CloseComponent();
            })));

        cut.Find("form").Submit();

        Assert.Contains("custom-errors", cut.Markup);
        Assert.Contains("Name is required", cut.Markup);
    }

    // -------------------------------------------------------------------------
    // Criterion 11 & 12: MariloField renders label and invalid CSS class
    // -------------------------------------------------------------------------

    [Fact]
    public void Field_WithText_RendersLabelElement()
    {
        var cut = Render<MariloField>(p => p
            .Add(f => f.Text, "My Field"));

        Assert.Contains("<label", cut.Markup);
        Assert.Contains("My Field", cut.Markup);
    }

    [Fact]
    public void Field_WithNoText_DoesNotRenderLabelElement()
    {
        var cut = Render<MariloField>(p => p
            .Add(f => f.ChildContent, (RenderFragment)(b =>
            {
                b.OpenElement(0, "span");
                b.AddContent(1, "input placeholder");
                b.CloseElement();
            })));

        Assert.DoesNotContain("<label", cut.Markup);
    }

    [Fact]
    public void Field_AddsInvalidClass_WhenFieldHasValidationErrors()
    {
        var model = new TestModel();

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloField>(0);
                builder.AddAttribute(1, nameof(MariloField.Id), nameof(TestModel.Name));
                builder.AddAttribute(2, nameof(MariloField.Text), "Name");
                builder.CloseComponent();
            })));

        cut.Find("form").Submit();

        Assert.Contains("mar-field--invalid", cut.Markup);
    }

    // -------------------------------------------------------------------------
    // Criterion 13 & 14: MariloLabel supports Text and invalid CSS class
    // -------------------------------------------------------------------------

    [Fact]
    public void Label_WithText_RendersTextContent()
    {
        var cut = Render<MariloLabel>(p => p
            .Add(l => l.Text, "Email Address"));

        Assert.Contains("Email Address", cut.Markup);
        Assert.Contains("<label", cut.Markup);
    }

    [Fact]
    public void Label_AddsInvalidClass_WhenFieldHasValidationErrors()
    {
        var model = new TestModel();

        var cut = Render<MariloForm>(p => p
            .Add(f => f.Model, model)
            .Add(f => f.FormValidation, builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();
            })
            .Add(f => f.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloLabel>(0);
                builder.AddAttribute(1, nameof(MariloLabel.For), nameof(TestModel.Name));
                builder.AddAttribute(2, nameof(MariloLabel.Text), "Name");
                builder.CloseComponent();
            })));

        cut.Find("form").Submit();

        Assert.Contains("mar-label--invalid", cut.Markup);
    }

    // -------------------------------------------------------------------------
    // Criterion 15: Existing MariloValidation is retained (backward compatible)
    // -------------------------------------------------------------------------

    [Fact]
    public void MariloValidation_RendersWithMessageAndSeverity()
    {
        var cut = Render<MariloValidation>(p => p
            .Add(v => v.Message, "Something went wrong")
            .Add(v => v.Severity, Marilo.Core.Enums.ValidationSeverity.Error));

        Assert.Contains("Something went wrong", cut.Markup);
    }

    // -------------------------------------------------------------------------
    // Helper: captures cascaded EditContext from a MariloForm
    // -------------------------------------------------------------------------

    private class CascadingEditContextCapture : ComponentBase
    {
        [CascadingParameter] private EditContext? EditContext { get; set; }
        [Parameter] public EventCallback<EditContext> OnContextReceived { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (EditContext != null && OnContextReceived.HasDelegate)
                await OnContextReceived.InvokeAsync(EditContext);
        }
    }
}
