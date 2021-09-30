// <copyright file="Label.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Application.Excel
{
    using System;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public class Label : IControl
    {
        public Label(ICell cell)
        {
            this.Cell = cell;
        }

        public ISessionObject SessionObject { get; internal set; }

        public RoleType RoleType { get; internal set; }

        public RoleType DisplayRoleType { get; internal set; }

        public Func<object, dynamic> GetRelation { get; internal set; }

        public RoleType RelationType { get; internal set; }

        public ICell Cell { get; set; }

        public void Bind()
        {
            if (this.SessionObject.CanRead(this.DisplayRoleType ?? this.RoleType))
            {
                if (this.RelationType != null)
                {
                    var relation = (ISessionObject)this.SessionObject.Get(this.DisplayRoleType ?? this.RoleType);
                    if (relation != null)
                    {
                        if (relation.CanRead(this.RelationType))
                        {
                            this.SetCellValue(relation, this.RelationType);
                        }
                    }
                }
                else
                {
                    this.SetCellValue(this.SessionObject, this.DisplayRoleType ?? this.RoleType);
                }
            }
        }

        public void OnCellChanged()
        {
            // Restore the object value
            this.SetCellValue(this.SessionObject, this.RoleType);
        }

        public void Unbind()
        {
            // TODO
        }

        private void SetCellValue(ISessionObject obj, RoleType roleType)
        {
            if (roleType.ObjectType.ClrType == typeof(bool))
            {
                if (obj.Get(roleType) is bool boolvalue && boolvalue)
                {
                    this.Cell.Value = Constants.YES;
                }
                else
                {
                    this.Cell.Value = Constants.NO;
                }
            }
            else if (roleType.ObjectType.ClrType == typeof(DateTime))
            {
                var dt = (DateTime?)obj?.Get(roleType);
                this.Cell.Value = dt?.ToOADate();
            }
            else
            {
                this.Cell.Value = obj.Get(roleType);
            }
        }
    }
}
