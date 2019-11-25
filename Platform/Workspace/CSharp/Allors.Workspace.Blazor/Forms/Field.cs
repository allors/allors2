namespace Allors.Workspace.Blazor
{
    using Allors.Workspace.Meta;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Validation;

    public abstract class Field : ComponentBase, IAllorsValidationField
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        [CascadingParameter]
        public AllorsValidationFields ValidationFields { get; set; }

        [Parameter]
        public bool FullWidth { get; set; } = true;

        public abstract IPropertyType PropertyType { get; }

        public abstract ISessionObject Object { get; set; }

        public abstract FieldIdentifier FieldIdentifier { get;  }

        public abstract void Validate(ValidationMessageStore messages);
    }
}
