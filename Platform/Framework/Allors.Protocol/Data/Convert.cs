//------------------------------------------------------------------------------------------------- 
// <copyright file="Convert.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
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
