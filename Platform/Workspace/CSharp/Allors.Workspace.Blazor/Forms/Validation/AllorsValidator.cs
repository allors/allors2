namespace Allors.Workspace.Blazor.Validation
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class AllorsValidator : ComponentBase
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        [CascadingParameter]
        public AllorsValidationFields ValidationFields { get; set; }

        [Parameter]
        public IAllorsValidation Validation { get; set; }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (this.EditContext == null)
            {
                throw new InvalidOperationException($"{nameof(AllorsValidator)} requires a cascading parameter of type {nameof(this.EditContext)}.");
            }

            this.Validation ??= new DefaultAllorsValidation();

            var messages = new ValidationMessageStore(this.EditContext);

            // Perform object-level validation on request
            this.EditContext.OnValidationRequested += (sender, eventArgs) =>
            {
                this.Validation.OnValidationRequested(this.ValidationFields, messages);

                this.EditContext.NotifyValidationStateChanged();
            };

            // Perform per-field validation on each field edit
            this.EditContext.OnFieldChanged += (sender, eventArgs) =>
            {
                foreach (var field in this.ValidationFields.Fields.Where(v => v == eventArgs.FieldIdentifier.Model))
                {
                    this.Validation.OnFieldChanged(this.ValidationFields, field, messages);
                }

                this.EditContext.NotifyValidationStateChanged();
            };
        }
    }
}
