namespace Allors.Workspace.Blazor
{
    using Microsoft.AspNetCore.Components.Forms;

    public partial interface IAllorsValidationField
    {
        void Validate(ValidationMessageStore messages);
    }
}
