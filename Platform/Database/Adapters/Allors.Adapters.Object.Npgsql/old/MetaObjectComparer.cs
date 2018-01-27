//------------------------------------------------------------------------------------------------- 
// <copyright file="MetaObjectComparer.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the Serialization type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.Adapters
{
    using System.Xml;

    using Allors.Meta;

    /// <summary>
    /// Xml tag definitions and utility methods for Xml Serialization.
    /// An <see cref="IDatabase"/> is serialized to a <see cref="XmlDocument"/> 
    /// according to the Allors Serialization Xml Schema.
    /// </summary>
    public static class MetaObjectComparer
    {
        public static int ById(IMetaObject x, IMetaObject y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}