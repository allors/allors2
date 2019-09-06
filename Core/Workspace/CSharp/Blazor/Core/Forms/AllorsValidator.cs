namespace Allors.Blazor
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class AllorsValidator : ComponentBase
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        [CascadingParameter]
        public AllorsValidation Validation { get; set; }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (this.EditContext == null)
            {
                throw new InvalidOperationException($"{nameof(AllorsValidator)} requires a cascading parameter of type {nameof(EditContext)}.");
            }

            if (this.Validation == null)
            {
                throw new InvalidOperationException($"{nameof(AllorsValidator)} requires a cascading parameter of type {nameof(AllorsValidation)}.");
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
            editContext.NotifyValidationStateChanged();
        }

        private static void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier)
        {
            editContext.NotifyValidationStateChanged();
        }
    }
}
