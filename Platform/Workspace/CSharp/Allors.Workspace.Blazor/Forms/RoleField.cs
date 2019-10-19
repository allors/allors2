namespace Allors.Workspace.Blazor
{
    using System;
    using Meta;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class RoleField : Field, IDisposable
    {
        public ISessionObject Object { get; set; }

        public IRoleType RoleType { get; set; }

        public string AssignedName { get; set; }

        public bool AssignedDisabled { get; set; }

        public bool AssignedRequired { get; set; }

        public string AssignedLabel { get; set; }

        public bool Readonly { get; set; }

        public string Hint { get; set; }

        public bool Focus { get; set; }

        public bool EmptyStringIsNull { get; set; } = true;

        public bool ExistObject => this.Object != null;

        public object Model
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

        public bool CanRead => this.ExistObject && this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.ExistObject && this.Object.CanWrite(this.RoleType);

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

        public string Name => !string.IsNullOrWhiteSpace(this.AssignedName) ? this.AssignedName : this.RoleType.Name + '_' + this.Object?.Id.ToString("N");

        public string Label => !string.IsNullOrWhiteSpace(this.AssignedLabel) ? this.AssignedLabel : this.RoleType.Name; // TODO: Humanize(this.RoleType.Name);

        public bool Required => this.AssignedRequired ? this.AssignedRequired : this.RoleType.IsRequired;

        public bool Disable => !this.CanWrite || this.AssignedDisabled;

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

        void System.IDisposable.Dispose() => this.Validation.Remove(this);

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
                this.Validation.Add(this);
            }
        }
    }
}
