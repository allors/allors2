namespace Allors.Workspace.Blazor.Validation
{
    using Allors.Workspace.Meta;
    using Microsoft.AspNetCore.Components.Forms;

    public partial interface IField
    {
        ISessionObject Object { get; }

        IPropertyType PropertyType { get; }

        FieldIdentifier FieldIdentifier { get; }

        void Validate(ValidationMessageStore messages);
    }
}
