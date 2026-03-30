---
title: Message
page_title: Validation Tools - Message
description: Validation Tools - Message.
slug: validation-tools-message
tags: marilo,blazor,validation,tools,message
published: True
position: 15
components: ["validationmessage", "validationsummary", "validationtooltip"]
---
# Marilo Validation Message for Blazor

The [Marilo Validation Message for Blazor](https://www.marilo.com/blazor-ui/validation-message) adds built-in styling and customization options on top of the standard [.NET ValidationMessage](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.validationmessage-1), such as [`Template`](#template) and [`Class`](#class) parameters.

## Basics

To use a Marilo Validation Message:

1. Add the `<MariloValidationMessage>` tag.
1. Set the `For` parameter in the same way as with a standard Blazor `<ValidationMessage>`. There are two options:
    * If the `MariloValidationMessage` is in the same component as the Form or if the Form model object is available, use a lambda expression that points to a property of the Form model.
        ````RAZOR.skip-repl
        <MariloValidationMessage For="@(() => Customer.FirstName)" />

        @code {
            private CustomerModel Customer { get; set; } = new();

            public class CustomerModel
            {
                public string FirstName { get; set; } = string.Empty;
            }
        }
        ````
    * If the [validation message is in a child component](slug:inputs-kb-validate-child-component) that receives a `ValueExpression`, set the `For` parameter directly to the expression, without a lambda.
        ````RAZOR.skip-repl
        <MariloValidationMessage For="@FirstNameExpression" />

        @code {
            [Parameter]
            public Expression<System.Func<string>>? FirstNameExpression { get; set; }
        }
        ````

Refer to the following sections for additional information and examples with the [Marilo Form](#using-with-mariloform) and standard [Blazor `<EditForm>`](#using-with-editform).

## Using with MariloForm

The Marilo Form [displays inline validation messages by default if validation is enabled](slug:form-validation). You may need to define `<MariloValidationMessage>` components manually when you want to:

* Use [form item templates](slug:form-formitems-template). In this case, [add the validation message in the form item template](slug:form-formitems-template#example).
* Customize the validation messages, for example, change their rendering with a [validation message template](#template). In this case, add the validation message inside a [Form item template](slug:form-formitems-template#example).
* Customize the placement of the validation messages in the Form, so that they are outside the Form item containers. In this case, consider a [`<FormItemsTemplate>`](slug:form-formitems-formitemstemplate) that gives you full control over the Form rendering between the form items. Alternatively, consider a [Marilo ValidationSummary](slug:validation-tools-summary).

>caption Use Marilo ValidationMessage in a MariloForm

````RAZOR
@using System.ComponentModel.DataAnnotations

<MariloForm Model="@Employee"
             Width="300px">
    <FormValidation>
        <DataAnnotationsValidator />
    </FormValidation>
    <FormItems>
        <FormItem Field="@nameof(Person.FirstName)" LabelText="First Name">
            <Template>
                <label for="first-name" class="k-label k-form-label">First Name</label>
                <div class="k-form-field-wrap">
                    <MariloTextBox @bind-Value="@Employee.FirstName"
                                    Id="first-name" />
                    <MariloValidationMessage For="@(() => Employee.FirstName)" />
                </div>
            </Template>
        </FormItem>
        <FormItem Field="@nameof(Person.LastName)" LabelText="Last Name" />
    </FormItems>
</MariloForm>

@code {
    private Person Employee { get; set; } = new();

    public class Person
    {
        [Required(ErrorMessage = "Please enter a first name")]
        [MinLength(2, ErrorMessage = "The first name must be at least 2 characters long")]
        [MaxLength(40, ErrorMessage = "The first name must be up to 40 characters long")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
````

## Using with EditForm

In an existing Blazor `EditForm`, replace the `<ValidationMessage>` tags with `<MariloValidationMessage>` tags. The `For` parameter is set in the same way for both validation components.

>caption Use Marilo ValidationMessage in an EditForm

````RAZOR
@using System.ComponentModel.DataAnnotations

<EditForm Model="@Employee" style="width:300px">
    <DataAnnotationsValidator />

    <label for="first-name">First Name</label>
    <MariloTextBox @bind-Value="@Employee.FirstName" Id="first-name" />
    <MariloValidationMessage For="@(() => Employee.FirstName)" />

    <label for="last-name">Last Name</label>
    <MariloTextBox @bind-Value="@Employee.LastName" Id="last-name" />
    <MariloValidationMessage For="@(() => Employee.LastName)" />

    <div>
        <MariloButton>Submit</MariloButton>
    </div>
</EditForm>

@code {
    private Person Employee { get; set; } = new();

    public class Person
    {
        [Required(ErrorMessage = "Please enter a first name")]
        [MinLength(2, ErrorMessage = "The first name must be at least 2 characters long")]
        [MaxLength(40, ErrorMessage = "The first name must be up to 40 characters long")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
````

## Template

The Marilo ValidationMessage allows you to customize its rendering with a nested `<Template>` tag. The template `context` is an `IEnumerable<string>` collection of all messages for the validated model property.

>caption Using ValidationMessage Template

````RAZOR
@using System.ComponentModel.DataAnnotations

<MariloForm Model="@Employee"
             Width="300px">
    <FormValidation>
        <DataAnnotationsValidator />
    </FormValidation>
    <FormItems>
        <FormItem Field="@nameof(Person.FirstName)" LabelText="First Name">
            <Template>
                <label for="first-name" class="k-label k-form-label">First Name</label>
                <div class="k-form-field-wrap">
                    <MariloTextBox @bind-Value="@Employee.FirstName"
                                    Id="first-name" />
                    <MariloValidationMessage For="@(() => Employee.FirstName)">
                        <Template Context="validationMessages">
                            @foreach (string message in validationMessages)
                            {
                                <div>
                                    <span class="k-form-error k-invalid-msg" style="display:flex; gap: .4em;">
                                        <MariloSvgIcon Icon="@SvgIcon.ExclamationCircle" />
                                        @message
                                    </span>
                                </div>
                            }
                        </Template>
                    </MariloValidationMessage>
                </div>
            </Template>
        </FormItem>
        <FormItem Field="@nameof(Person.LastName)" LabelText="Last Name" />
    </FormItems>
</MariloForm>

@code {
    private Person Employee { get; set; } = new();

    public class Person
    {
        [Required(ErrorMessage = "Please enter a first name")]
        [MinLength(2, ErrorMessage = "The first name must be at least 2 characters long")]
        [MaxLength(40, ErrorMessage = "The first name must be up to 40 characters long")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
````

## Class

Use the `Class` parameter of the Validation Message to add a custom CSS class to the `span.k-form-error`. This element holds the validation message.

>caption Using MariloValidationMessage Class

````RAZOR.skip-repl
<MariloValidationMessage Class="bold-blue"
                          For="@(() => Employee.FirstName)" />

<style>
    .bold-blue {
        font-weight: bold;
        color: blue;
    }
</style>
````

## Next Steps

* Explore [MariloValidationTooltip](slug:validation-tools-tooltip)

## See Also

* [Live Demo: Validation](https://demos.marilo.com/blazor-ui/validation/overview)
* [Validate Inputs in Child Components](slug:inputs-kb-validate-child-component)
* [Marilo ValidationSummary](slug:validation-tools-summary)
* [Marilo ValidationTooltip](slug:validation-tools-tooltip)
