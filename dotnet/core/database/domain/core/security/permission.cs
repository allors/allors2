// <copyright file="Permission.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Text;

    using Allors.Meta;

    using Resources;

    public partial class Permission
    {
        public ObjectType ConcreteClass
        {
            get => (ObjectType)this.Strategy.Session.Database.MetaPopulation.Find(this.ConcreteClassPointer);

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

        public bool ExistConcreteClass => this.ConcreteClass != null;

        public bool ExistOperandType => this.ExistOperandTypePointer;

        public bool ExistOperation => this.ExistOperationEnum;

        public IOperandType OperandType
        {
            get => (IOperandType)this.Strategy.Session.Database.MetaPopulation.Find(this.OperandTypePointer);

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

        public Operations Operation
        {
            get => (Operations)this.OperationEnum;

            set => this.OperationEnum = (int)value;
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
                        derivation.Validation.AddError(this, this.Meta.OperationEnum, DomainErrors.PermissionOnlyReadForRoleOrAssociationType);
                    }

                    break;

                case Operations.Write:
                    // Write Operations should only be allowed on RoleTypes
                    if (!(this.OperandType is RoleType))
                    {
                        derivation.Validation.AddError(this, this.Meta.OperationEnum, DomainErrors.PermissionOnlyWriteForRoleType);
                    }

                    break;

                case Operations.Execute:
                    // Execute Operations should only be allowed on MethodTypes
                    if (!(this.OperandType is MethodType))
                    {
                        derivation.Validation.AddError(this, this.Meta.OperationEnum, DomainErrors.PermissionOnlyExecuteForMethodType);
                    }

                    break;

                default:
                    derivation.Validation.AddError(this, this.Meta.OperationEnum, "Illegal enum");
                    break;
            }
        }

        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                foreach (Role role in this.RolesWherePermission)
                {
                    iteration.AddDependency(role, this);
                    iteration.Mark(role);
                }

                this.Strategy.Session.ClearCache<PermissionCache>();
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

        internal void Sync(ObjectType concreteClass, IOperandType operandType, Operations operation)
        {
            this.OperandType = operandType;
            this.Operation = operation;
            this.ConcreteClassPointer = concreteClass.Id;
        }
    }
}
