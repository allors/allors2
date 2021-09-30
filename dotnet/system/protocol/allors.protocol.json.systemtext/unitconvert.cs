// <copyright file="Convert.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.SystemTextJson
{
    using System;
    using System.Globalization;
    using System.Text.Json;
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

        public object FromJson(string tag, object value)
        {
            switch (value)
            {
                case null:
                    return null;
                case string @string:
                    return tag switch
                    {
                        UnitTags.DateTime => XmlConvert.ToDateTime(@string, XmlDateTimeSerializationMode.Utc),
                        UnitTags.Binary => Convert.FromBase64String(@string),
                        UnitTags.Boolean => XmlConvert.ToBoolean(@string),
                        UnitTags.Decimal => XmlConvert.ToDecimal(@string),
                        UnitTags.Float => XmlConvert.ToDouble(@string),
                        UnitTags.Integer => XmlConvert.ToInt32(@string),
                        UnitTags.String => @string,
                        UnitTags.Unique => XmlConvert.ToGuid(@string),
                        _ => throw new Exception($"{@string} not supported for tag {tag}")
                    };
                default:
                {
                    var element = (JsonElement)value;
                    return element.ValueKind switch
                    {
                        JsonValueKind.Null => null,
                        JsonValueKind.Undefined => null,
                        JsonValueKind.False => false,
                        JsonValueKind.True => true,
                        _ => tag switch
                        {
                            UnitTags.DateTime => element.GetDateTime(),
                            UnitTags.Binary => Convert.FromBase64String(element.GetString()),
                            UnitTags.Decimal => element.ValueKind == JsonValueKind.String
                                ? Convert.ToDecimal(element.GetString(), CultureInfo.InvariantCulture)
                                : element.GetDecimal(),
                            UnitTags.Float => element.GetDouble(),
                            UnitTags.Integer => element.GetInt32(),
                            UnitTags.String => element.GetString(),
                            UnitTags.Unique => element.GetGuid(),
                            _ => throw new Exception($"{element.ValueKind} not supported for tag {tag}")
                        }
                    };
                }
            }
        }
    }
}
