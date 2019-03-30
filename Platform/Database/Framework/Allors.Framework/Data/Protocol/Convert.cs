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

    using Allors.Meta;

    public static class Convert
    {
        public static object ToValue(IUnit unit, string @string)
        {
            if (@string == null)
            {
                return null;
            }

            switch (unit.UnitTag)
            {
                case UnitTags.Binary:
                    return System.Convert.FromBase64String(@string);

                case UnitTags.Boolean:
                    return XmlConvert.ToBoolean(@string);

                case UnitTags.DateTime:
                    return XmlConvert.ToDateTime(@string, XmlDateTimeSerializationMode.Utc);

                case UnitTags.Decimal:
                    return XmlConvert.ToDecimal(@string);

                case UnitTags.Float:
                    return XmlConvert.ToDouble(@string);

                case UnitTags.Integer:
                    return XmlConvert.ToInt32(@string);

                case UnitTags.String:
                    return @string;

                case UnitTags.Unique:
                    return XmlConvert.ToGuid(@string);

                default:
                    throw new Exception("Unknown unit " + unit);
            }
        }
    }
}