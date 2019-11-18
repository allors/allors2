namespace Allors.Workspace.Blazor
{
    using System;
    using Meta;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class RoleField : Field, IDisposable
    {
        private ISessionObject @object;
        private string name;
        private bool disable;
        private bool required;
        private string label;

        [Parameter]
        public ISessionObject Object
        {
            get => this.@object ?? (ISessionObject)this.EditContext?.Model;
            set => this.@object = value;
        }

        public bool ExistObject => this.Object != null;

        [Parameter]
        public IRoleType RoleType { get; set; }

        public bool CanRead => this.ExistObject && this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.ExistObject && this.Object.CanWrite(this.RoleType);

        public virtual object Model
        {
            get => this.ExistObject ? this.Object.Get(this.RoleType) : null;

            set
            {
                if (this.ExistObject)
                {

                    if (value is string stringValue && this.EmptyStringIsNull && string.IsNullOrWhiteSpace(stringValue))
                    {
                        value = null;
                    }

                    this.Object.Set(this.RoleType, value);
                }
            }
        }

        [Parameter]
        public string Name
        {
            get => !string.IsNullOrWhiteSpace(this.name)
                    ? this.name
                    : this.RoleType.Name + '_' + this.Object?.Id;
            set => this.name = value;
        }

        [Parameter]
        public string Label
        {
            get => !string.IsNullOrWhiteSpace(this.label) ? this.label : this.RoleType.Name; // TODO: Humanize(this.RoleType.Name);
            set => this.label = value;
        }

        [Parameter]
        public bool ShowLabel { get; set; } = true;

        [Parameter]
        public bool Disable
        {
            get => this.disable || !this.CanWrite;
            set => this.disable = value;
        }

        [Parameter]
        public bool Required
        {
            get => this.required ? this.required : this.RoleType.IsRequired;
            set => this.required = value;
        }

        [Parameter]
        public bool Readonly { get; set; }

        [Parameter]
        public string Hint { get; set; }

        [Parameter]
        public bool Focus { get; set; }

        [Parameter]
        public bool EmptyStringIsNull { get; set; } = true;

        public string TextType
        {
            get
            {
                if (this.RoleType.ObjectType.Id == UnitIds.Integer ||
                    this.RoleType.ObjectType.Id == UnitIds.Decimal ||
                    this.RoleType.ObjectType.Id == UnitIds.Float)
                {
                    return "number";
                }

                return "text";

            }
        }

        public string DataAllorsId => this.Object?.Id.ToString("D");

        public string DataAllorsRoleType => this.RoleType?.Id.ToString("D");

        public FieldIdentifier FieldIdentifier => new FieldIdentifier(this, "Model");

        public void Add(ISessionObject value)
        {
            if (this.ExistObject)
            {
                this.Object.Add(this.RoleType, value);
            }
        }

        public void Remove(ISessionObject value)
        {
            if (this.ExistObject)
            {
                this.Object.Remove(this.RoleType, value);
            }
        }

        void IDisposable.Dispose() => this.Validation.Remove(this);

        public override void Validate(ValidationMessageStore messages)
        {
            if (this.RoleType.IsRequired && this.Model == null)
            {
                messages.Add(this.FieldIdentifier, $"{this.RoleType.Name} is required");
            }
            else
            {
                messages.Clear(this.FieldIdentifier);
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                this.Validation?.Add(this);
            }
        }
    }
}
