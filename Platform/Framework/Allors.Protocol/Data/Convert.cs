//-------------------------------------------------------------------------------------------------
// <copyright file="Convert.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Protocol.Data
{
    using System;
    using System.Xml;

    public static partial class DataConvert
    {
        public static string ToString(object value)
        {
            switch (value)
            {
                case null:
                    return null;
                case string @string:
                    return @string;
                case int @int:
                    return XmlConvert.ToString(@int);
                case decimal @decimal:
                    return XmlConvert.ToString(@decimal);
                case double @double:
                    return XmlConvert.ToString(@double);
                case bool @bool:
                    return XmlConvert.ToString(@bool);
                case DateTime dateTime:
                    return XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.Utc);
                case Guid @guid:
                    return XmlConvert.ToString(@guid);
                case byte[] binary:
                    return System.Convert.ToBase64String(binary);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
