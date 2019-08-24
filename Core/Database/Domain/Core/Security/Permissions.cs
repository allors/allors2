// <copyright file="Permissions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using Allors;
    using Allors.Meta;

    public partial class Permissions
    {
        // TODO: Cache permissions
        public Permission Get(Class @class, OperandType operand, Operations operation)
        {
            var extent = this.Extent();
            extent.Filter.AddEquals(this.Meta.ConcreteClassPointer, @class.Id);
            extent.Filter.AddEquals(this.Meta.OperandTypePointer, operand.Id);
            extent.Filter.AddEquals(this.Meta.OperationEnum, operation);

            return extent.First;
        }

        public void Sync()
        {
            var permissionByOperationByConcreteClassByOperandType = new Dictionary<OperandType, Dictionary<ObjectType, Dictionary<Operations, Permission>>>();

            foreach (Permission permission in new Permissions(this.Session).Extent())
            {
                if (permission.OperandType == null || !permission.ExistConcreteClass || !permission.ExistOperation)
                {
                    permission.Delete();
                    continue;
                }

                if (!permissionByOperationByConcreteClassByOperandType.TryGetValue(permission.OperandType, out var permissionByOperationByConcreteClass))
                {
                    permissionByOperationByConcreteClass = new Dictionary<ObjectType, Dictionary<Operations, Permission>>();
                    permissionByOperationByConcreteClassByOperandType[permission.OperandType] = permissionByOperationByConcreteClass;
                }

                if (!permissionByOperationByConcreteClass.TryGetValue(permission.ConcreteClass, out var permissionByOperation))
                {
                    permissionByOperation = new Dictionary<Operations, Permission>();
                    permissionByOperationByConcreteClass.Add(permission.ConcreteClass, permissionByOperation);
                }

                permissionByOperation[permission.Operation] = permission;
            }

            var domain = (MetaPopulation)this.Session.Database.ObjectFactory.MetaPopulation;
            foreach (var relationType in domain.RelationTypes)
            {
                {
                    // AssociationType
                    var associationType = relationType.AssociationType;
                    permissionByOperationByConcreteClassByOperandType.TryGetValue(associationType, out var permissionByOperationByConcreteClass);

                    if (associationType.RelationType.RoleType.ObjectType is Composite composite)
                    {
                        foreach (var concreteClass in composite.Classes)
                        {
                            Dictionary<Operations, Permission> permissionByOperation = null;
                            if (permissionByOperationByConcreteClass != null)
                            {
                                permissionByOperationByConcreteClass.TryGetValue(concreteClass, out permissionByOperation);
                            }

                            Operations[] operations = { Operations.Read };
                            foreach (var operation in operations)
                            {
                                Permission permission = null;
                                if (permissionByOperation != null)
                                {
                                    permissionByOperation.TryGetValue(operation, out permission);
                                }

                                if (permission == null)
                                {
                                    permission = new PermissionBuilder(this.Session).Build();
                                }

                                permission.Sync(concreteClass, associationType, operation);
                            }
                        }
                    }
                }

                {
                    // RoleType
                    var roleType = relationType.RoleType;
                    permissionByOperationByConcreteClassByOperandType.TryGetValue(roleType, out var permissionByOperationByConcreteClass);

                    foreach (var concreteClass in roleType.RelationType.AssociationType.ObjectType.Classes)
                    {
                        Dictionary<Operations, Permission> permissionByOperation = null;
                        if (permissionByOperationByConcreteClass != null)
                        {
                            permissionByOperationByConcreteClass.TryGetValue(concreteClass, out permissionByOperation);
                        }

                        var operations = new[] { Operations.Read, Operations.Write };

                        foreach (var operation in operations)
                        {
                            Permission permission = null;
                            if (permissionByOperation != null)
                            {
                                permissionByOperation.TryGetValue(operation, out permission);
                            }

                            if (operation == Operations.Write && roleType.RelationType.IsDerived)
                            {
                                if (permission != null)
                                {
                                    permission.Delete();
                                }
                            }
                            else
                            {
                                if (permission == null)
                                {
                                    permission = new PermissionBuilder(this.Session).Build();
                                }

                                permission.Sync(concreteClass, roleType, operation);
                            }
                        }
                    }
                }
            }

            foreach (var methodType in domain.MethodTypes)
            {
                permissionByOperationByConcreteClassByOperandType.TryGetValue(methodType, out var permissionByOperationByConcreteClass);

                foreach (var concreteClass in methodType.ObjectType.Classes)
                {
                    Dictionary<Operations, Permission> permissionByOperation = null;
                    if (permissionByOperationByConcreteClass != null)
                    {
                        permissionByOperationByConcreteClass.TryGetValue(concreteClass, out permissionByOperation);
                    }

                    Permission permission = null;
                    if (permissionByOperation != null)
                    {
                        permissionByOperation.TryGetValue(Operations.Execute, out permission);
                    }

                    if (permission == null)
                    {
                        permission = new PermissionBuilder(this.Session).Build();
                    }

                    permission.Sync(concreteClass, methodType, Operations.Execute);
                }
            }
        }

        protected override void CoreSetup(Setup setup)
        {
            if (setup.Config.SetupSecurity)
            {
                this.Sync();
            }
        }
    }
}
