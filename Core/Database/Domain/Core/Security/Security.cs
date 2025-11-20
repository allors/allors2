// <copyright file="Security.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors;
    using Allors.Meta;

    public partial class Security
    {
        private static readonly Operations[] ReadWriteExecute = { Operations.Read, Operations.Write, Operations.Execute };

        private readonly Dictionary<Guid, Dictionary<Guid, Permission>> deniablePermissionByOperandTypeIdByObjectTypeId;
        private readonly Dictionary<Guid, Dictionary<IOperandType, Permission>> executePermissionsByObjectTypeId;
        private readonly Dictionary<ObjectType, IObjects> objectsByObjectType;
        private readonly Dictionary<Guid, Dictionary<IOperandType, Permission>> readPermissionsByObjectTypeId;
        private readonly Dictionary<Guid, Role> roleById;
        private readonly ISession session;
        private readonly Dictionary<Guid, Dictionary<IOperandType, Permission>> writePermissionsByObjectTypeId;

        public Security(ISession session)
        {
            this.session = session;

            this.objectsByObjectType = new Dictionary<ObjectType, IObjects>();
            foreach (ObjectType objectType in session.Database.MetaPopulation.Composites)
            {
                this.objectsByObjectType[objectType] = objectType.GetObjects(session);
            }

            this.roleById = new Dictionary<Guid, Role>();
            foreach (Role role in session.Extent<Role>())
            {
                if (!role.ExistUniqueId)
                {
                    throw new Exception("Role " + role + " has no unique id");
                }

                this.roleById[role.UniqueId] = role;
            }

            this.readPermissionsByObjectTypeId = new Dictionary<Guid, Dictionary<IOperandType, Permission>>();
            this.writePermissionsByObjectTypeId = new Dictionary<Guid, Dictionary<IOperandType, Permission>>();
            this.executePermissionsByObjectTypeId = new Dictionary<Guid, Dictionary<IOperandType, Permission>>();

            this.deniablePermissionByOperandTypeIdByObjectTypeId = new Dictionary<Guid, Dictionary<Guid, Permission>>();

            foreach (Permission permission in session.Extent<Permission>())
            {
                if (!permission.ExistConcreteClassPointer || !permission.ExistOperandTypePointer || !permission.ExistOperation)
                {
                    throw new Exception("Permission " + permission + " has no concrete class, operand type and/or operation");
                }

                var objectId = permission.ConcreteClassPointer;

                if (permission.Operation != Operations.Read)
                {
                    var operandType = permission.OperandTypePointer;

                    if (!this.deniablePermissionByOperandTypeIdByObjectTypeId.TryGetValue(objectId, out var deniablePermissionByOperandTypeId))
                    {
                        deniablePermissionByOperandTypeId = new Dictionary<Guid, Permission>();
                        this.deniablePermissionByOperandTypeIdByObjectTypeId[objectId] = deniablePermissionByOperandTypeId;
                    }

                    deniablePermissionByOperandTypeId.Add(operandType, permission);
                }

                Dictionary<Guid, Dictionary<IOperandType, Permission>> permissionByOperandTypeByObjectTypeId;
                switch (permission.Operation)
                {
                    case Operations.Read:
                        permissionByOperandTypeByObjectTypeId = this.readPermissionsByObjectTypeId;
                        break;

                    case Operations.Write:
                        permissionByOperandTypeByObjectTypeId = this.writePermissionsByObjectTypeId;
                        break;

                    case Operations.Execute:
                        permissionByOperandTypeByObjectTypeId = this.executePermissionsByObjectTypeId;
                        break;

                    default:
                        throw new Exception("Unkown operation: " + permission.Operation);
                }

                if (!permissionByOperandTypeByObjectTypeId.TryGetValue(objectId, out var permissionByOperandType))
                {
                    permissionByOperandType = new Dictionary<IOperandType, Permission>();
                    permissionByOperandTypeByObjectTypeId[objectId] = permissionByOperandType;
                }

                if (permission.OperandType == null)
                {
                    permission.Delete();
                }
                else
                {
                    permissionByOperandType.Add(permission.OperandType, permission);
                }
            }
        }

        public void Apply()
        {
            foreach (Role role in this.session.Extent<Role>())
            {
                role.RemovePermissions();
                role.RemoveDeniedPermissions();
            }

            this.OnPreSetup();

            foreach (var objects in this.objectsByObjectType.Values)
            {
                objects.Secure(this);
            }

            this.OnPostSetup();

            this.session.Derive();
        }

        public void Deny(ObjectType objectType, ObjectState objectState, params Operations[] operations)
        {
            var actualOperations = operations ?? ReadWriteExecute;
            foreach (var operation in actualOperations)
            {
                Dictionary<IOperandType, Permission> permissionByOperandType;
                switch (operation)
                {
                    case Operations.Read:
                        this.readPermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                        break;

                    case Operations.Write:
                        this.writePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                        break;

                    case Operations.Execute:
                        this.executePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                        break;

                    default:
                        throw new Exception("Unkown operation: " + operations);
                }

                if (permissionByOperandType != null)
                {
                    foreach (var dictionaryEntry in permissionByOperandType)
                    {
                        objectState.AddDeniedPermission(dictionaryEntry.Value);
                    }
                }
            }
        }

        public void Deny(ObjectType objectType, ObjectState objectState, params OperandType[] operandTypes) => this.Deny(objectType, objectState, (IEnumerable<OperandType>)operandTypes);

        public void Deny(ObjectType objectType, ObjectState objectState, IEnumerable<OperandType> operandTypes)
        {
            if (this.deniablePermissionByOperandTypeIdByObjectTypeId.TryGetValue(objectType.Id, out var deniablePermissionByOperandTypeId))
            {
                foreach (var operandType in operandTypes)
                {
                    if (deniablePermissionByOperandTypeId.TryGetValue(operandType.Id, out var permission))
                    {
                        objectState.AddDeniedPermission(permission);
                    }
                }
            }
        }

        public void DenyExcept(ObjectType objectType, ObjectState objectState, IEnumerable<IOperandType> excepts, params Operations[] operations)
        {
            var actualOperations = operations ?? ReadWriteExecute;
            foreach (var operation in actualOperations)
            {
                Dictionary<IOperandType, Permission> permissionByOperandType;
                switch (operation)
                {
                    case Operations.Read:
                        this.readPermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                        break;

                    case Operations.Write:
                        this.writePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                        break;

                    case Operations.Execute:
                        this.executePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                        break;

                    default:
                        throw new Exception("Unkown operation: " + operations);
                }

                if (permissionByOperandType != null)
                {
                    foreach (var dictionaryEntry in permissionByOperandType.Where(v => !excepts.Contains(v.Key)))
                    {
                        objectState.AddDeniedPermission(dictionaryEntry.Value);
                    }
                }
            }
        }

        public void Grant(Guid roleId, ObjectType objectType, params Operations[] operations)
        {
            if (this.roleById.TryGetValue(roleId, out var role))
            {
                var actualOperations = operations ?? ReadWriteExecute;
                foreach (var operation in actualOperations)
                {
                    Dictionary<IOperandType, Permission> permissionByOperandType;
                    switch (operation)
                    {
                        case Operations.Read:
                            this.readPermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        case Operations.Write:
                            this.writePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        case Operations.Execute:
                            this.executePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        default:
                            throw new Exception("Unkown operation: " + operations);
                    }

                    if (permissionByOperandType != null)
                    {
                        foreach (var dictionaryEntry in permissionByOperandType)
                        {
                            role.AddPermission(dictionaryEntry.Value);
                        }
                    }
                }
            }
        }

        public void Grant(Guid roleId, ObjectType objectType, ConcreteRoleType concreteRoleType, params Operations[] operations) => this.Grant(roleId, objectType, concreteRoleType.RoleType, operations);

        public void Grant(Guid roleId, ObjectType objectType, IOperandType operandType, params Operations[] operations)
        {
            if (this.roleById.TryGetValue(roleId, out var role))
            {
                var actualOperations = operations ?? ReadWriteExecute;
                foreach (var operation in actualOperations)
                {
                    Dictionary<IOperandType, Permission> permissionByOperandType;
                    switch (operation)
                    {
                        case Operations.Read:
                            this.readPermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        case Operations.Write:
                            this.writePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        case Operations.Execute:
                            this.executePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        default:
                            throw new Exception("Unkown operation: " + operations);
                    }

                    if (permissionByOperandType != null)
                    {
                        if (permissionByOperandType.TryGetValue(operandType, out var permission))
                        {
                            role.AddPermission(permission);
                        }
                    }
                }
            }
        }

        public void GrantAdministrator(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.AdministratorId, objectType, operations);

        public void GrantCreator(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.CreatorId, objectType, operations);

        public void GrantExcept(Guid roleId, ObjectType objectType, ICollection<IOperandType> excepts, params Operations[] operations)
        {
            if (this.roleById.TryGetValue(roleId, out var role))
            {
                var actualOperations = operations ?? ReadWriteExecute;
                foreach (var operation in actualOperations)
                {
                    Dictionary<IOperandType, Permission> permissionByOperandType;
                    switch (operation)
                    {
                        case Operations.Read:
                            this.readPermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        case Operations.Write:
                            this.writePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        case Operations.Execute:
                            this.executePermissionsByObjectTypeId.TryGetValue(objectType.Id, out permissionByOperandType);
                            break;

                        default:
                            throw new Exception("Unkown operation: " + operations);
                    }

                    if (permissionByOperandType != null)
                    {
                        foreach (var dictionaryEntry in permissionByOperandType.Where(v => !excepts.Contains(v.Key)))
                        {
                            role.AddPermission(dictionaryEntry.Value);
                        }
                    }
                }
            }
        }

        public void GrantGuest(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.GuestId, objectType, operations);

        public void GrantGuestCreator(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.GuestCreatorId, objectType, operations);

        public void GrantOwner(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.OwnerId, objectType, operations);

        private void CoreOnPostSetup()
        {
        }

        private void CoreOnPreSetup()
        {
        }
    }
}
