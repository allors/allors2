// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Permission.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System.Text;

    using Allors.Meta;

    using Resources;

    public partial class Permission
    {
        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.IsModified(this))
            {
                foreach (Role role in this.RolesWherePermission)
                {
                    derivation.AddDependency(role, this);
                }

                this.Strategy.Session.ClearCache<PermissionCache>();
            }
        }

        public void CoreOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            switch (this.Operation)
            {
                case Operations.Read:
                    // Read Operations should only be allowed on AssociaitonTypes && RoleTypes
                    if (!(this.OperandType is RoleType || this.OperandType is AssociationType))
                    {
                        derivation.Validation.AddError(this, Meta.OperationEnum, DomainErrors.PermissionOnlyReadForRoleOrAssociationType);
                    }

                    break;

                case Operations.Write:
                    // Write Operations should only be allowed on RoleTypes
                    if (!(this.OperandType is RoleType))
                    {
                        derivation.Validation.AddError(this, Meta.OperationEnum, DomainErrors.PermissionOnlyWriteForRoleType);
                    }

                    break;

                case Operations.Execute:
                    // Execute Operations should only be allowed on MethodTypes
                    if (!(this.OperandType is MethodType))
                    {
                        derivation.Validation.AddError(this, Meta.OperationEnum, DomainErrors.PermissionOnlyExecuteForMethodType);
                    }

                    break;

                default:
                    derivation.Validation.AddError(this, Meta.OperationEnum, "Illegal enum");
                    break;
            }
        }

        public OperandType OperandType
        {
            get
            {
                return (OperandType)this.Strategy.Session.Database.MetaPopulation.Find(this.OperandTypePointer);
            }

            set
            {
                if (value == null)
                {
                    this.RemoveOperandTypePointer();
                }
                else
                {
                    this.OperandTypePointer = value.Id;
                }
            }
        }

        public bool ExistOperandType
        {
            get
            {
                return this.ExistOperandTypePointer;
            }
        }

        public Operations Operation
        {
            get
            {
                return (Operations)this.OperationEnum;
            }

            set
            {
                this.OperationEnum = (int)value;
            }
        }

        public bool ExistOperation
        {
            get
            {
                return this.ExistOperationEnum;
            }
        }

        public ObjectType ConcreteClass
        {
            get
            {
                return (ObjectType)this.Strategy.Session.Database.MetaPopulation.Find(this.ConcreteClassPointer);
            }

            set
            {
                if (value == null)
                {
                    this.RemoveConcreteClassPointer();
                }
                else
                {
                    this.ConcreteClassPointer = value.Id;
                }
            }
        }

        public bool ExistConcreteClass
        {
            get
            {
                return this.ConcreteClass != null;
            }
        }

        public override string ToString()
        {
            var toString = new StringBuilder();
            if (this.ExistOperation)
            {
                var operation = this.Operation;
                toString.Append(operation);
            }
            else
            {
                toString.Append("[missing operation]");
            }

            toString.Append(" for ");

            toString.Append(this.ExistOperandType ? this.OperandType.GetType().Name + ":" + this.OperandType : "[missing operand]");

            return toString.ToString();
        }

        internal void Sync(ObjectType concreteClass, OperandType operandType, Operations operation)
        {
            this.OperandType = operandType;
            this.Operation = operation;
            this.ConcreteClassPointer = concreteClass.Id;
        }
    }
}
