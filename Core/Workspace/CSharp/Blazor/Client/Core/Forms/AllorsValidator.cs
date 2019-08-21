namespace Allors.Workspace
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public class AllorsValidator : ComponentBase
    {
        [CascadingParameter]
        public EditContext CurrentEditContext { get; set; }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (this.CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(AllorsValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(AllorsValidator)} " +
                    $"inside an EditForm.");
            }

            var messages = new ValidationMessageStore(this.CurrentEditContext);

            // Perform object-level validation on request
            this.CurrentEditContext.OnValidationRequested +=
                (sender, eventArgs) => ValidateModel(this.CurrentEditContext, messages);

            // Perform per-field validation on each field edit
            this.CurrentEditContext.OnFieldChanged +=
                (sender, eventArgs) => ValidateField(this.CurrentEditContext, messages, eventArgs.FieldIdentifier);
        }

        private static void ValidateModel(EditContext editContext, ValidationMessageStore messages)
        {
            messages.Clear();
            editContext.NotifyValidationStateChanged();
        }

        private static void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier)
        {
            editContext.NotifyValidationStateChanged();
        }
    }
}
