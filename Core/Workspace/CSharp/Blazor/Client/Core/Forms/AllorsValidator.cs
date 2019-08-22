namespace Allors.Workspace
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public class AllorsValidator : ComponentBase
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (this.EditContext == null)
            {
                throw new InvalidOperationException($"{nameof(AllorsValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(AllorsValidator)} " +
                    $"inside an EditForm.");
            }

            var messages = new ValidationMessageStore(this.EditContext);

            // Perform object-level validation on request
            this.EditContext.OnValidationRequested +=
                (sender, eventArgs) => ValidateModel(this.EditContext, messages);

            // Perform per-field validation on each field edit
            this.EditContext.OnFieldChanged +=
                (sender, eventArgs) => ValidateField(this.EditContext, messages, eventArgs.FieldIdentifier);
        }

        private static void ValidateModel(EditContext editContext, ValidationMessageStore messages)
        {
            messages.Clear();
            messages.Add(editContext.Field("FirstName"), "First Name is required");
            editContext.NotifyValidationStateChanged();
        }

        private static void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier)
        {
            editContext.NotifyValidationStateChanged();
        }
    }
}
