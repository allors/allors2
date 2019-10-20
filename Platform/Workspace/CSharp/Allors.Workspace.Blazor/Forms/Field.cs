namespace Allors.Workspace.Blazor
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public abstract class Field : ComponentBase, IAllorsValidationField
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        [CascadingParameter]
        public AllorsValidation Validation { get; set; }

        [Parameter]
        public bool FullWidth { get; set; } = true;

        public abstract void Validate(ValidationMessageStore messages);
    }
}
