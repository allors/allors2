namespace Allors.Workspace.Blazor
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;
    using Markdig.Helpers;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Validation;

    public abstract class Field : ComponentBase, IField
    {
        [CascadingParameter]
        public EditContext EditContext { get; set; }

        [CascadingParameter]
        public Fields ValidationFields { get; set; }

        [Parameter]
        public bool FullWidth { get; set; } = true;

        [Parameter]
        public bool HideValidation { get; set; } = true;

        [Parameter]
        public string ValidMessage { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> Attributes { get; set; }

        public abstract IPropertyType PropertyType { get; }

        public abstract IObject Object { get; set; }

        public abstract FieldIdentifier FieldIdentifier { get; }

        public abstract void Validate(ValidationMessageStore messages);
    }
}
