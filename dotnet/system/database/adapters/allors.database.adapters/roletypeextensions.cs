// <copyright file="RoleTypeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System;

    using Meta;

    public static class RoleTypeExtensions
    {
        /// <summary>
        /// Assert that the unit is compatible with the IObjectType of the RoleType.
        /// </summary>
        /// <param name="roleType">
        ///     The role type.
        /// </param>
        /// <param name="unit">
        ///     The unit to normalize.
        /// </param>
        /// <returns>
        /// The normalized unit.
        /// </returns>
        public static object Normalize(this IRoleType roleType, object unit)
        {
            if (unit == null)
            {
                return null;
            }

            var unitType = (IUnit)roleType.ObjectType;
            var unitTypeTag = unitType.Tag;

            var normalizedUnit = unit;

            switch (unitTypeTag)
            {
                case UnitTags.String:
                    if (!(unit is string))
                    {
                        throw new ArgumentException("RoleType is not a String.");
                    }

                    var stringUnit = (string)unit;
                    var size = roleType.Size;
                    if (size > -1 && stringUnit.Length > size)
                    {
                        throw new ArgumentException("Size of relationType " + roleType + " is too big (" + stringUnit.Length + ">" + size + ").");
                    }

                    break;
                case UnitTags.Integer:
                    if (!(unit is int))
                    {
                        throw new ArgumentException("RoleType is not an Integer.");
                    }

                    break;
                case UnitTags.DateTime:
                    if (unit is DateTime)
                    {
                        var dateTime = (DateTime)normalizedUnit;
                        if (dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue)
                        {
                            switch (dateTime.Kind)
                            {
                                case DateTimeKind.Local:
                                    dateTime = dateTime.ToUniversalTime();
                                    break;
                                case DateTimeKind.Unspecified:
                                    throw new ArgumentException("DateTime value is of DateTimeKind.Kind Unspecified. \nUnspecified is only allowed for DateTime.MaxValue and DateTime.MinValue, use DateTimeKind.Utc or DateTimeKind.Local instead.");
                            }

                            normalizedUnit = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
                        }
                    }
                    else if (!(unit is DateTime))
                    {
                        throw new ArgumentException("RoleType is not a DateTime.");
                    }

                    break;
                case UnitTags.Decimal:
                    if (unit is int || unit is long || unit is float || unit is double)
                    {
                        normalizedUnit = Convert.ToDecimal(unit);
                    }
                    else if (!(unit is decimal))
                    {
                        throw new ArgumentException("RoleType is not a Decimal.");
                    }

                    break;
                case UnitTags.Float:
                    if (unit is int || unit is long || unit is float)
                    {
                        normalizedUnit = Convert.ToDouble(unit);
                    }
                    else if (!(unit is double))
                    {
                        throw new ArgumentException("RoleType is not a Float.");
                    }

                    break;
                case UnitTags.Boolean:
                    if (!(unit is bool))
                    {
                        throw new ArgumentException("RoleType is not a Boolean.");
                    }

                    break;
                case UnitTags.Unique:
                    if (!(unit is Guid))
                    {
                        throw new ArgumentException("RoleType is not a Boolean.");
                    }

                    break;
                case UnitTags.Binary:
                    if (!(unit is byte[]))
                    {
                        throw new ArgumentException("RoleType is not a Boolean.");
                    }

                    break;
                default:
                    throw new ArgumentException("Unknown Unit IObjectType: " + unitTypeTag);
            }

            return normalizedUnit;
        }
    }
}
