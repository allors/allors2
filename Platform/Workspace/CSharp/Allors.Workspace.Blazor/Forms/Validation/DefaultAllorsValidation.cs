namespace Allors.Workspace.Blazor.Validation
{
    using Microsoft.AspNetCore.Components.Forms;

    public partial class DefaultAllorsValidation : IAllorsValidation
    {
        public void OnValidationRequested(AllorsValidationFields fields, ValidationMessageStore messages)
        {
            messages.Clear();
            foreach (var field in fields.Fields)
            {
                field.Validate(messages);
            }
        }

        public void OnFieldChanged(AllorsValidationFields fields, IAllorsValidationField field, ValidationMessageStore messages) => field.Validate(messages);
    }
}
