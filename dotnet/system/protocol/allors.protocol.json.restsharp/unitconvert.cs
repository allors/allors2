// <copyright file="Convert.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.RestSharp
{
    using System;
    using System.Globalization;
    using System.Xml;

    public class UnitConvert : IUnitConvert
    {
        public object ToJson(object value) =>
            value switch
            {
                DateTime dateTime => dateTime,
                byte[] binary => Convert.ToBase64String(binary),
                bool @bool => @bool,
                decimal @decimal => @decimal.ToString(CultureInfo.InvariantCulture),
                double @double => @double,
                int @int => @int,
                string @string => @string,
                Guid @guid => @guid.ToString("D"),
                null => null,
                _ => throw new ArgumentException()
            };

        public object FromJson(string tag, object value) =>
            value switch
            {
                null => null,
                string stringValue => tag switch
                {
                    UnitTags.DateTime => XmlConvert.ToDateTime(stringValue, XmlDateTimeSerializationMode.Utc),
                    UnitTags.Binary => Convert.FromBase64String(stringValue),
                    UnitTags.Boolean => XmlConvert.ToBoolean(stringValue),
                    UnitTags.Decimal => XmlConvert.ToDecimal(stringValue),
                    UnitTags.Float => XmlConvert.ToDouble(stringValue),
                    UnitTags.Integer => XmlConvert.ToInt32(stringValue),
                    UnitTags.String => value,
                    UnitTags.Unique => XmlConvert.ToGuid(stringValue),
                    _ => throw new Exception($"Can not convert from string for unit type with tag {tag}"),
                },
                int intValue => tag switch
                {
                    UnitTags.Decimal => Convert.ToDecimal(intValue),
                    UnitTags.Float => Convert.ToDouble(intValue),
                    UnitTags.Integer => intValue,
                    _ => throw new Exception($"Can not convert from long to unit type with tag {tag}"),
                },
                long longValue => tag switch
                {
                    UnitTags.Decimal => Convert.ToDecimal(longValue),
                    UnitTags.Float => Convert.ToDouble(longValue),
                    UnitTags.Integer => Convert.ToInt32(longValue),
                    _ => throw new Exception($"Can not convert from long to unit type with tag {tag}"),
                },
                float floatValue => tag switch
                {
                    UnitTags.Decimal => Convert.ToDecimal(floatValue),
                    UnitTags.Float => Convert.ToDouble(floatValue),
                    _ => throw new Exception($"Can not convert from long to unit type with tag {tag}"),
                },
                double doubleValue => tag switch
                {
                    UnitTags.Decimal => Convert.ToDecimal(doubleValue),
                    UnitTags.Float => doubleValue,
                    _ => throw new Exception($"Can not convert from long to unit type with tag {tag}"),
                },
                DateTime dateValue => tag switch
                {
                    UnitTags.DateTime => dateValue,
                    _ => throw new Exception($"Can not convert from DateTime to unit type with tag {tag}"),
                },
                bool boolValue => tag switch
                {
                    UnitTags.Boolean => boolValue,
                    _ => throw new Exception($"Can not convert from bool to unit type with tag {tag}"),
                },
                _ => value,
            };
    }
}
