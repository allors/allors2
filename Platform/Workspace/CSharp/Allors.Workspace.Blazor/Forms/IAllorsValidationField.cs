namespace Allors.Workspace.Blazor
{
    using Allors.Workspace.Meta;
    using Microsoft.AspNetCore.Components.Forms;

    public partial interface IAllorsValidationField
    {
        ISessionObject Object { get; }

        IPropertyType PropertyType { get; }

        FieldIdentifier FieldIdentifier { get; }

        void Validate(ValidationMessageStore messages);
    }
}
