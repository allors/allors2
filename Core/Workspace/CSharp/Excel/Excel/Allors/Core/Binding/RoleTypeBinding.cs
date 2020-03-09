// <copyright file="RoleTypeBinding.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Dipu.Excel;

    public class RoleTypeBinding : IBinding
    {
        public RoleTypeBinding(ISessionObject @object, RoleType roleType, RoleType relationType = null, bool oneWayBinding = false)
        {
            this.Object = @object;
            this.RoleType = roleType;
            this.RelationType = relationType;
            this.OneWayBinding = oneWayBinding;
        }

        public ISessionObject Object { get; }

        /// <summary>
        /// Gets the RoleType we are Binding to.
        /// </summary>
        public RoleType RoleType { get; }

        /// <summary>
        /// Gets or sets the RoleType we want to use as Display Value.
        /// </summary>
        public RoleType DisplayRoleType { get; set; }

        /// <summary>
        /// Gets the RelationType from the RoleType reference object we want to use as Display Value
        /// </summary>
        public RoleType RelationType { get; }

        public bool OneWayBinding { get; }

        public bool TwoWayBinding => !this.OneWayBinding;

        /// <summary>
        /// Gets a functions that maps the value in the cell to a reference to a relation. eg Lookup WheelDiameter by its inch value.
        /// </summary>
        public System.Func<object, dynamic> GetRelation { get; internal set; }

        /// <summary>
        /// Gets the function that transforms the value in the cell to something else. eg true => Yes.
        /// </summary>
        public System.Func<object, dynamic> Transform { get; internal set; }

        public void ToCell(ICell cell)
        {
            if (this.RelationType != null)
            {
                var relation = (dynamic)this.Object.Get(this.DisplayRoleType ?? this.RoleType);

                if (this.Transform == null)
                {
                    if (this.RelationType.ObjectType.ClrType == typeof(System.DateTime))
                    {
                        var dt = (System.DateTime?)relation?.Get(this.RelationType);
                        cell.Value = dt?.ToOADate();
                    }
                    else
                    {
                        cell.Value = relation?.Get(this.RelationType);
                    }
                }
                else
                {
                    if(relation != null)
                    {
                        cell.Value = this.Transform(relation);
                    }
                    else
                    {
                        cell.Value = null;
                    }
                }
            }
            else
            {
                if (this.Transform == null)
                {
                    if (this.RoleType.ObjectType.ClrType == typeof(System.DateTime))
                    {
                        var dt = (System.DateTime?)this.Object.Get(this.DisplayRoleType ?? this.RoleType);
                        cell.Value = dt?.ToOADate();
                    }
                    else
                    {
                        cell.Value = this.Object.Get(this.DisplayRoleType ?? this.RoleType);
                    }
                }
                else
                {
                    cell.Value = this.Transform(this.Object.Get(this.DisplayRoleType ?? this.RoleType));
                }
            }
        }

        public void ToDomain(ICell cell)
        {
            if (this.TwoWayBinding)
            {
                if (this.GetRelation == null)
                {
                    this.Object.Set(this.RoleType, cell.Value);
                }
                else
                {
                    var relation = this.GetRelation(cell.Value);
                    this.Object.Set(this.RoleType, relation);
                }
            }
        }
    }
}
