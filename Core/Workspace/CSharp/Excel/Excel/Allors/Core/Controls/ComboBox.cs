// <copyright file="ComboBox.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Application.Excel
{
    using System;
    using System.Globalization;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public class ComboBox : IControl
    {
        public ComboBox(ICell cell)
        {
            this.Cell = cell;
        }

        public Range Options => this.Cell.Options;

        public ISessionObject SessionObject { get; internal set; }

        public RoleType RoleType { get; internal set; }

        public RoleType DisplayRoleType { get; internal set; }

        public Func<object, dynamic> ToDomain { get; internal set; }

        public ICell Cell { get; set; }

        public RoleType RelationType { get; internal set; }

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

            this.Cell.Style = this.SessionObject.CanWrite(this.RoleType) ? Constants.WriteStyle : Constants.ReadOnlyStyle;
        }

        public void OnCellChanged()
        {
            if (this.SessionObject.CanWrite(this.RoleType))
            {
                if (this.ToDomain == null)
                {
                    if (this.RoleType.ObjectType.ClrType == typeof(bool))
                    {
                        if (Constants.YES.Equals(Convert.ToString(this.Cell.Value, CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase))
                        {
                            this.SessionObject.Set(this.RoleType, true);
                        }
                        else
                        {
                            this.SessionObject.Set(this.RoleType, false);
                        }
                    }
                    else if (this.RoleType.ObjectType.ClrType == typeof(DateTime))
                    {
                        if (double.TryParse(Convert.ToString(this.Cell.Value, CultureInfo.CurrentCulture), out double d))
                        {
                            var dt = DateTime.FromOADate(d);
                            this.SessionObject.Set(this.RoleType, dt);
                        }   
                        else
                        {
                            if (this.RoleType.IsRequired)
                            {
                                this.SessionObject.Set(this.RoleType, DateTime.MinValue);
                            }
                            else
                            {
                                this.SessionObject.Set(this.RoleType, null);
                            }
                        }
                    }
                    else
                    {
                        this.SessionObject.Set(this.RoleType, this.Cell.Value);
                    }
                }
                else
                {
                    var relation = this.ToDomain(this.Cell.Value);
                    this.SessionObject.Set(this.RoleType, relation);
                }

                this.Cell.Style = Constants.ChangedStyle;
            }
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
