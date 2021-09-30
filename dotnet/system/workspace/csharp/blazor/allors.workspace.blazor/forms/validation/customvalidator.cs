namespace Allors.Workspace.Blazor.Validation
{
    using System;
    using System.Collections;
    using System.Linq;
    using Meta;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public abstract class CustomValidator : ComponentBase
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
                    this.Validate(field, messages);
                }

                this.EditContext.NotifyValidationStateChanged();
            };

            // Perform per-field validation on each field edit
            this.EditContext.OnFieldChanged += (sender, eventArgs) =>
            {
                var field = this.Fields.Items.First(v => v == eventArgs.FieldIdentifier.Model);
                this.Validate(field, messages);

                this.EditContext.NotifyValidationStateChanged();
            };
        }

        protected void AssertExists(IField field, ValidationMessageStore messages, IPropertyType propertyType)
        {
            if (field.PropertyType == propertyType)
            {
                var model = field.Object;
                var roleType = field.PropertyType as IRoleType;

                if (roleType?.IsOne == true)
                {
                    if (model.Strategy.GetRole(roleType) == null)
                    {
                        this.AddShouldExistMessage(field, messages);
                    }
                }
                else if (roleType?.IsMany == true)
                {
                    if (((ICollection)model.Strategy.GetRole(roleType)).Count == 0)
                    {
                        this.AddShouldExistMessage(field, messages);
                    }
                }
            }
        }

        protected void AssertNotExists(IField field, ValidationMessageStore messages, IPropertyType propertyType)
        {
            if (field.PropertyType == propertyType)
            {
                var model = field.Object;
                var roleType = field.PropertyType as IRoleType;

                if (roleType?.IsOne == true)
                {
                    if (model.Strategy.GetRole(roleType) != null)
                    {
                        this.AddShouldNotExistMessage(field, messages);
                    }
                }
                else if (roleType?.IsMany == true)
                {
                    if (((ICollection)model.Strategy.GetRole(roleType)).Count > 0)
                    {
                        this.AddShouldNotExistMessage(field, messages);
                    }
                }
            }
        }

        protected virtual void AddShouldExistMessage(IField field, ValidationMessageStore messages) => messages.Add(field.FieldIdentifier, $"{field.PropertyType.Name} is requried");

        protected virtual void AddShouldNotExistMessage(IField field, ValidationMessageStore messages) => messages.Add(field.FieldIdentifier, $"{field.PropertyType.Name} is not allowed");

        protected abstract void Validate(IField field, ValidationMessageStore messages);
    }
}
