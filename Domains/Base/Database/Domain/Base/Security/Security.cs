// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Security.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors;
    using Allors.Meta;

    public partial class Security
    {
        private static readonly Operations[] ReadWriteExecute = { Operations.Read, Operations.Write, Operations.Execute };

        private readonly ISession session;
        private readonly Dictionary<ObjectType, IObjects> objectsByObjectType;

        private readonly Dictionary<Guid, Role> roleById;
        private readonly Dictionary<Guid, Dictionary<OperandType, Permission>> readPermissionsByObjectTypeId;
        private readonly Dictionary<Guid, Dictionary<OperandType, Permission>> writePermissionsByObjectTypeId;
        private readonly Dictionary<Guid, Dictionary<OperandType, Permission>> executePermissionsByObjectTypeId;

        private readonly Dictionary<Guid, Dictionary<Guid, Permission>> deniablePermissionByOperandTypeIdByObjectTypeId;

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

            this.readPermissionsByObjectTypeId = new Dictionary<Guid, Dictionary<OperandType, Permission>>();
            this.writePermissionsByObjectTypeId = new Dictionary<Guid, Dictionary<OperandType, Permission>>();
            this.executePermissionsByObjectTypeId = new Dictionary<Guid, Dictionary<OperandType, Permission>>();

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

                    Dictionary<Guid, Permission> deniablePermissionByOperandTypeId;
                    if (!this.deniablePermissionByOperandTypeIdByObjectTypeId.TryGetValue(objectId, out deniablePermissionByOperandTypeId))
                    {
                        deniablePermissionByOperandTypeId = new Dictionary<Guid, Permission>();
                        this.deniablePermissionByOperandTypeIdByObjectTypeId[objectId] = deniablePermissionByOperandTypeId;
                    }

                    deniablePermissionByOperandTypeId.Add(operandType, permission);
                }

                Dictionary<Guid, Dictionary<OperandType, Permission>> permissionByOperandTypeByObjectTypeId;
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

                Dictionary<OperandType, Permission> permissionByOperandType;
                if (!permissionByOperandTypeByObjectTypeId.TryGetValue(objectId, out permissionByOperandType))
                {
                    permissionByOperandType = new Dictionary<OperandType, Permission>();
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

            foreach (var objects in this.objectsByObjectType.Values)
            {
                objects.Secure(this);
            }

            this.session.Derive(true);
        }

        public void Deny(ObjectType objectType, ObjectState objectState, params Operations[] operations)
        {
            var actualOperations = operations ?? ReadWriteExecute;
            foreach (var operation in actualOperations)
            {
                Dictionary<OperandType, Permission> permissionByOperandType;
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

        public void Deny(ObjectType objectType, ObjectState objectState, params OperandType[] operandTypes)
        {
            this.Deny(objectType, objectState, (IEnumerable<OperandType>)operandTypes);
        }

        public void Deny(ObjectType objectType, ObjectState objectState, IEnumerable<OperandType> operandTypes)
        {
            Dictionary<Guid, Permission> deniablePermissionByOperandTypeId;
            if (this.deniablePermissionByOperandTypeIdByObjectTypeId.TryGetValue(objectType.Id, out deniablePermissionByOperandTypeId))
            {
                foreach (var operandType in operandTypes)
                {
                    Permission permission;
                    if (deniablePermissionByOperandTypeId.TryGetValue(operandType.Id, out permission))
                    {
                        objectState.AddDeniedPermission(permission);
                    }
                }
            }
        }

        public void Grant(Guid roleId, IObjectType objectType, params Operations[] operations)
        {
            Role role;
            if (this.roleById.TryGetValue(roleId, out role))
            {
                var actualOperations = operations ?? ReadWriteExecute;
                foreach (var operation in actualOperations)
                {
                    Dictionary<OperandType, Permission> permissionByOperandType;
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

        public void Grant(Guid roleId, IObjectType objectType, OperandType operandType, params Operations[] operations)
        {
            Role role;
            if (this.roleById.TryGetValue(roleId, out role))
            {
                var actualOperations = operations ?? ReadWriteExecute;
                foreach (var operation in actualOperations)
                {
                    Dictionary<OperandType, Permission> permissionByOperandType;
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
                        Permission permission;
                        if (permissionByOperandType.TryGetValue(operandType, out permission))
                        {
                            role.AddPermission(permission);
                        }
                    }
                }
            }
        }

        public void GrantAdministrator(IObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.AdministratorId, objectType, operations);
        }

        public void GrantGuest(IObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.GuestId, objectType, operations);
        }

        public void GrantCreator(IObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.CreatorId, objectType, operations);
        }

        public void GrantOwner(IObjectType objectType, params Operations[] operations)
        {
            this.Grant(Roles.OwnerId, objectType, operations);
        }
    }
}