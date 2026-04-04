using Bunit;
using Marilo.Components.Layout;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Layout;

public class WizardTests : MariloTestBase
{
    // ── Helper: render a MariloWizard with N simple steps ──────────────────

    private IRenderedComponent<MariloWizard> RenderWizard(
        int stepCount = 2,
        Action<ComponentParameterCollectionBuilder<MariloWizard>>? extra = null)
    {
        return Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.ChildContent, BuildSteps(stepCount));
            extra?.Invoke(parameters);
        });
    }

    private static RenderFragment BuildSteps(int count) => builder =>
    {
        for (var i = 0; i < count; i++)
        {
            var seq = i * 4;
            var labelText = $"Step {i + 1}";
            var contentText = $"Content {i + 1}";
            builder.OpenComponent<WizardStep>(seq);
            builder.AddAttribute(seq + 1, "Label", labelText);
            builder.AddAttribute(seq + 2, "ChildContent", (RenderFragment)(b => b.AddContent(0, contentText)));
            builder.CloseComponent();
        }
    };

    // ── 1. Step registration ────────────────────────────────────────────────

    [Fact]
    public void StepRegistration_StepsAppearWhenPlacedAsChildren()
    {
        var cut = RenderWizard(3);

        var indicators = cut.FindAll("[role='tab']");
        Assert.Equal(3, indicators.Count);
    }

    // ── 2. Step labels render in stepper bar ────────────────────────────────

    [Fact]
    public void StepLabels_RenderInStepperBar()
    {
        var cut = RenderWizard(2);

        Assert.Contains("Step 1", cut.Markup);
        Assert.Contains("Step 2", cut.Markup);
    }

    // ── 3. Active step content renders in tabpanel ──────────────────────────

    [Fact]
    public void ActiveStepContent_RendersInTabpanel()
    {
        var cut = RenderWizard(2);

        var panel = cut.Find("[role='tabpanel']");
        Assert.Contains("Content 1", panel.InnerHtml);
    }

    // ── 4. Value=0 shows first step content ────────────────────────────────

    [Fact]
    public void Value_Zero_ShowsFirstStepContent()
    {
        var cut = Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.Value, 0);
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "First");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "First Content")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(4);
                builder.AddAttribute(5, "Label", "Second");
                builder.AddAttribute(6, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Second Content")));
                builder.CloseComponent();
            }));
        });

        var panel = cut.Find("[role='tabpanel']");
        Assert.Contains("First Content", panel.InnerHtml);
        Assert.DoesNotContain("Second Content", panel.InnerHtml);
    }

    // ── 5. Value two-way binding via ValueChanged ───────────────────────────

    [Fact]
    public void ValueChanged_FiresWhenNavigating()
    {
        var receivedValues = new List<int>();

        var cut = RenderWizard(3, p =>
            p.Add(x => x.ValueChanged,
                EventCallback.Factory.Create<int>(this, v => receivedValues.Add(v))));

        cut.Find(".mar-btn--primary").Click(); // Next

        Assert.Contains(1, receivedValues);
    }

    // ── 6. NextStep / PreviousStep navigation via button clicks ─────────────

    [Fact]
    public void NextButton_AdvancesToNextStep()
    {
        var cut = RenderWizard(3);

        cut.Find(".mar-btn--primary").Click();

        var tabs = cut.FindAll("[role='tab']");
        Assert.Equal("true", tabs[1].GetAttribute("aria-selected"));
    }

    [Fact]
    public void PreviousButton_ReturnsToPreviousStep()
    {
        var cut = RenderWizard(3);

        // Go to step 2
        cut.Find(".mar-btn--primary").Click();
        // Go back
        cut.Find(".mar-btn--flat").Click();

        var tabs = cut.FindAll("[role='tab']");
        Assert.Equal("true", tabs[0].GetAttribute("aria-selected"));
    }

    // ── 7. OnStepChange fires on navigation ─────────────────────────────────

    [Fact]
    public void OnStepChange_FiresOnNavigation()
    {
        int? changedTo = null;

        var cut = RenderWizard(3, p =>
            p.Add(x => x.OnStepChange,
                EventCallback.Factory.Create<int>(this, v => changedTo = v)));

        cut.Find(".mar-btn--primary").Click();

        Assert.Equal(1, changedTo);
    }

    // ── 8. Disabled step cannot be navigated to ─────────────────────────────

    [Fact]
    public void DisabledStep_CannotBeNavigatedTo_ViaClick()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "Active");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Active Content")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(4);
                builder.AddAttribute(5, "Label", "Disabled");
                builder.AddAttribute(6, "Disabled", true);
                builder.AddAttribute(7, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Disabled Content")));
                builder.CloseComponent();
            })));

        var tabs = cut.FindAll("[role='tab']");
        // The disabled tab should have disabled attribute
        Assert.NotNull(tabs[1].GetAttribute("disabled"));
    }

    // ── 9. Linear=true prevents clicking non-visited steps ──────────────────

    [Fact]
    public void Linear_True_PreventsClickingNonVisitedSteps()
    {
        var cut = Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.Linear, true);
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "S1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C1")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(4);
                builder.AddAttribute(5, "Label", "S2");
                builder.AddAttribute(6, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C2")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(8);
                builder.AddAttribute(9, "Label", "S3");
                builder.AddAttribute(10, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C3")));
                builder.CloseComponent();
            }));
        });

        // Step 3 (index 2) should be disabled because it hasn't been visited
        var tabs = cut.FindAll("[role='tab']");
        Assert.NotNull(tabs[2].GetAttribute("disabled"));
    }

    // ── 10. Linear=false allows clicking any step ────────────────────────────

    [Fact]
    public void Linear_False_AllowsClickingAnyStep()
    {
        var cut = Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.Linear, false);
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "S1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C1")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(4);
                builder.AddAttribute(5, "Label", "S2");
                builder.AddAttribute(6, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C2")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(8);
                builder.AddAttribute(9, "Label", "S3");
                builder.AddAttribute(10, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C3")));
                builder.CloseComponent();
            }));
        });

        // Step 3 (index 2) should NOT be disabled when Linear=false
        var tabs = cut.FindAll("[role='tab']");
        Assert.Null(tabs[2].GetAttribute("disabled"));
    }

    // ── 11. WizardButtons custom render fragment replaces default buttons ────

    [Fact]
    public void WizardButtons_CustomFragment_ReplacesDefaultButtons()
    {
        var cut = Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.ChildContent, BuildSteps(2));
            parameters.Add(p => p.WizardButtons,
                (RenderFragment<int>)(stepIndex => b => b.AddContent(0, $"Custom-{stepIndex}")));
        });

        Assert.Contains("Custom-0", cut.Markup);
        Assert.DoesNotContain("mar-btn--primary", cut.Markup);
        Assert.DoesNotContain("mar-btn--flat", cut.Markup);
    }

    // ── 12. WizardSteps wrapper — steps register through wrapper ─────────────

    [Fact]
    public void WizardSteps_Wrapper_StepsRegisterThroughWrapper()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloWizardSteps>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
                {
                    inner.OpenComponent<WizardStep>(0);
                    inner.AddAttribute(1, "Label", "W1");
                    inner.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "WC1")));
                    inner.CloseComponent();

                    inner.OpenComponent<WizardStep>(4);
                    inner.AddAttribute(5, "Label", "W2");
                    inner.AddAttribute(6, "ChildContent", (RenderFragment)(b => b.AddContent(0, "WC2")));
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            })));

        var tabs = cut.FindAll("[role='tab']");
        Assert.Equal(2, tabs.Count);
    }

    // ── 13. Width parameter renders as inline style ──────────────────────────

    [Fact]
    public void Width_Parameter_RendersAsInlineStyle()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.Width, "800px"));

        Assert.Contains("width:800px", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    // ── 14. Height parameter renders as inline style ─────────────────────────

    [Fact]
    public void Height_Parameter_RendersAsInlineStyle()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.Height, "600px"));

        Assert.Contains("height:600px", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    // ── 15. ShowPager renders "Step X of Y" text ─────────────────────────────

    [Fact]
    public void ShowPager_True_RendersStepOfYText()
    {
        var cut = RenderWizard(3, p => p.Add(x => x.ShowPager, true));

        Assert.Contains("Step 1 of 3", cut.Markup);
    }

    // ── 16. StepperPosition adds CSS class modifier ──────────────────────────

    [Fact]
    public void StepperPosition_Left_AddsCssClassModifier()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.StepperPosition, WizardStepperPosition.Left));

        Assert.Contains("mar-wizard--stepper-left", cut.Markup);
    }

    [Fact]
    public void StepperPosition_Bottom_AddsCssClassModifier()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.StepperPosition, WizardStepperPosition.Bottom));

        Assert.Contains("mar-wizard--stepper-bottom", cut.Markup);
    }

    // ── 17. Content parameter takes priority over ChildContent ───────────────

    [Fact]
    public void Content_Parameter_TakesPriorityOverChildContent()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "Step");
                builder.AddAttribute(2, "Content", (RenderFragment)(b => b.AddContent(0, "From Content")));
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "From ChildContent")));
                builder.CloseComponent();
            })));

        var panel = cut.Find("[role='tabpanel']");
        Assert.Contains("From Content", panel.InnerHtml);
        Assert.DoesNotContain("From ChildContent", panel.InnerHtml);
    }

    // ── 18. Text parameter overrides step number ─────────────────────────────

    [Fact]
    public void Text_Parameter_OverridesStepNumber()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Text", "A");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C1")));
                builder.CloseComponent();
            })));

        Assert.Contains(">A<", cut.Markup);
        Assert.DoesNotContain(">1<", cut.Markup);
    }

    // ── 19. Optional parameter shows "(Optional)" text ───────────────────────

    [Fact]
    public void Optional_True_ShowsOptionalText()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "Opt Step");
                builder.AddAttribute(2, "Optional", true);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C")));
                builder.CloseComponent();
            })));

        Assert.Contains("(Optional)", cut.Markup);
    }

    // ── 20. Valid=true shows check icon, Valid=false shows error icon ─────────

    [Fact]
    public void Valid_True_ShowsCheckIcon()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "V");
                builder.AddAttribute(2, "Valid", (bool?)true);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C")));
                builder.CloseComponent();
            })));

        Assert.Contains("mar-wizard__step-icon--valid", cut.Markup);
    }

    [Fact]
    public void Valid_False_ShowsErrorIcon()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "V");
                builder.AddAttribute(2, "Valid", (bool?)false);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C")));
                builder.CloseComponent();
            })));

        Assert.Contains("mar-wizard__step-icon--invalid", cut.Markup);
    }

    // ── 21. StepTemplate replaces default step indicator ─────────────────────

    [Fact]
    public void StepTemplate_ReplacesDefaultStepIndicator()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "S1");
                builder.AddAttribute(2, "StepTemplate",
                    (RenderFragment)(b => b.AddContent(0, "CUSTOM_INDICATOR")));
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C1")));
                builder.CloseComponent();
            })));

        Assert.Contains("CUSTOM_INDICATOR", cut.Markup);
        Assert.DoesNotContain("mar-wizard__step-indicator", cut.Markup);
    }

    // ── 22. OnChange cancellation prevents step change ────────────────────────

    [Fact]
    public void OnChange_Cancellation_PreventsStepChange()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "S1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C1")));
                builder.AddAttribute(3, "OnChange",
                    EventCallback.Factory.Create<WizardStepChangeEventArgs>(this, args =>
                    {
                        args.IsCancelled = true;
                    }));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(4);
                builder.AddAttribute(5, "Label", "S2");
                builder.AddAttribute(6, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C2")));
                builder.CloseComponent();
            })));

        // Click Next — OnChange cancels it
        cut.Find(".mar-btn--primary").Click();

        // Should still be on step 1
        var panel = cut.Find("[role='tabpanel']");
        Assert.Contains("C1", panel.InnerHtml);
        Assert.DoesNotContain("C2", panel.InnerHtml);
    }

    // ── 23. Disabled step disables the Next navigation button ────────────────

    [Fact]
    public void NextButton_IsDisabled_WhenNextStepIsDisabled()
    {
        var cut = Render<MariloWizard>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "S1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C1")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(4);
                builder.AddAttribute(5, "Label", "S2");
                builder.AddAttribute(6, "Disabled", true);
                builder.AddAttribute(7, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C2")));
                builder.CloseComponent();
            })));

        var nextBtn = cut.Find(".mar-btn--primary");
        Assert.NotNull(nextBtn.GetAttribute("disabled"));
    }

    // ── 24. ARIA attributes: role=tablist, role=tab, role=tabpanel ───────────

    [Fact]
    public void Aria_Tablist_Role_IsPresent()
    {
        var cut = RenderWizard(2);
        Assert.NotNull(cut.Find("[role='tablist']"));
    }

    [Fact]
    public void Aria_Tab_Role_IsPresentOnEachStep()
    {
        var cut = RenderWizard(2);
        var tabs = cut.FindAll("[role='tab']");
        Assert.Equal(2, tabs.Count);
    }

    [Fact]
    public void Aria_Tabpanel_Role_IsPresent()
    {
        var cut = RenderWizard(2);
        Assert.NotNull(cut.Find("[role='tabpanel']"));
    }

    [Fact]
    public void Aria_Controls_MatchesTabpanelId()
    {
        var cut = RenderWizard(2);
        var tab = cut.Find("[role='tab']");
        var panelId = cut.Find("[role='tabpanel']").GetAttribute("id");

        Assert.Equal(panelId, tab.GetAttribute("aria-controls"));
    }

    [Fact]
    public void Aria_Current_Step_SetOnActiveTab()
    {
        var cut = RenderWizard(2);
        var activeTabs = cut.FindAll("[aria-current='step']");
        Assert.Single(activeTabs);
    }

    // ── 25. Finish button renders on last step and fires OnFinish ─────────────

    [Fact]
    public void FinishButton_RendersOnLastStep_AndFiresOnFinish()
    {
        var finished = false;

        var cut = Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.Value, 1);
            parameters.Add(p => p.OnFinish,
                EventCallback.Factory.Create(this, () => finished = true));
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "Label", "S1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C1")));
                builder.CloseComponent();

                builder.OpenComponent<WizardStep>(4);
                builder.AddAttribute(5, "Label", "S2");
                builder.AddAttribute(6, "ChildContent", (RenderFragment)(b => b.AddContent(0, "C2")));
                builder.CloseComponent();
            }));
        });

        // On last step, "Finish" should appear instead of "Next"
        Assert.Contains("Finish", cut.Markup);
        Assert.DoesNotContain("mar-btn--primary\">Next", cut.Markup.Replace(" ", "").Replace("\n", ""));

        cut.Find(".mar-btn--primary").Click();

        Assert.True(finished);
    }

    // ── Bonus: FinishText / NextText / PreviousText custom labels ────────────

    [Fact]
    public void CustomButtonText_FinishText_Renders()
    {
        var cut = Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.Value, 0);
            parameters.Add(p => p.FinishText, "Complete");
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<WizardStep>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Only step")));
                builder.CloseComponent();
            }));
        });

        Assert.Contains("Complete", cut.Markup);
    }

    [Fact]
    public void CustomButtonText_NextText_Renders()
    {
        var cut = Render<MariloWizard>(parameters =>
        {
            parameters.Add(p => p.NextText, "Proceed");
            parameters.Add(p => p.ChildContent, BuildSteps(2));
        });

        Assert.Contains("Proceed", cut.Markup);
    }
}
