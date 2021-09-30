// <copyright file="Convert.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;
    using System.Globalization;
    using System.Xml;
    using Meta;

    public static class UnitConvert
    {
        public static object Parse(Guid objectTypeId, string value)
        {
            if (value == null)
            {
                return null;
            }

            switch (objectTypeId)
            {
                case var u when u == UnitIds.DateTime:
                    return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc);
                case var u when u == UnitIds.Binary:
                    return Convert.FromBase64String(value);
                case var u when u == UnitIds.Boolean:
                    return XmlConvert.ToBoolean(value);
                case var u when u == UnitIds.Decimal:
                    return XmlConvert.ToDecimal(value);
                case var u when u == UnitIds.Float:
                    return XmlConvert.ToDouble(value);
                case var u when u == UnitIds.Integer:
                    return XmlConvert.ToInt32(value);
                case var u when u == UnitIds.String:
                    return value;
                case var u when u == UnitIds.Unique:
                    return XmlConvert.ToGuid(value);
                default:
                    throw new Exception($"Unknown unit type with id {objectTypeId}");
            }
        }

        public static string ToString(object value)
        {
            switch (value)
            {
                case DateTime dateTime:
                    return dateTime.ToString("o");
                case byte[] binary:
                    return Convert.ToBase64String(binary);
                case bool @bool:
                    return @bool ? "true" : "false";
                case decimal @decimal:
                    return @decimal.ToString(CultureInfo.InvariantCulture);
                case double @double:
                    return @double.ToString(CultureInfo.InvariantCulture);
                case int @int:
                    return @int.ToString(CultureInfo.InvariantCulture);
                case string @string:
                    return @string;
                case Guid @guid:
                    return @guid.ToString("D");
                case null:
                    return null;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
