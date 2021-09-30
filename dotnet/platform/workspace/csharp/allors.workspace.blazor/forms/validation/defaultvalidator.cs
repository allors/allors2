namespace Allors.Workspace.Blazor.Validation
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class DefaultValidator : ComponentBase
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        [CascadingParameter]
        public Fields Fields { get; set; }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (this.EditContext == null)
            {
                throw new InvalidOperationException($"{nameof(DefaultValidator)} requires cascading parameters {nameof(this.EditContext)} and {nameof(this.Fields)}.");
            }

            var messages = new ValidationMessageStore(this.EditContext);

            // Perform object-level validation on request
            this.EditContext.OnValidationRequested += (sender, eventArgs) =>
            {
                messages.Clear();
                foreach (var field in this.Fields.Items)
                {
                    field.Validate(messages);
                }

                this.EditContext.NotifyValidationStateChanged();
            };

            // Perform per-field validation on each field edit
            this.EditContext.OnFieldChanged += (sender, eventArgs) =>
            {
                foreach (var field in this.Fields.Items.Where(v => v == eventArgs.FieldIdentifier.Model))
                {
                    field.Validate(messages);
                }

                this.EditContext.NotifyValidationStateChanged();
            };
        }
    }
}
