namespace Allors.Workspace.Blazor.Validation
{
    using Microsoft.AspNetCore.Components.Forms;

    public partial interface IAllorsValidation
    {
        void OnValidationRequested(AllorsValidationFields fields, ValidationMessageStore messages);

        void OnFieldChanged(AllorsValidationFields fields, IAllorsValidationField field, ValidationMessageStore messages);
    }
}
