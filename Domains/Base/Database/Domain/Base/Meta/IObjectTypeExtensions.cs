//-------------------------------------------------------------------------------------------------
// <copyright file="IObjectTypeExtensions.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;
    using Allors.Meta;

    public static class IObjectTypeExtensions
    {
        public static IObjects GetObjects(this ObjectType objectType, ISession session)
        {
            var objectFactory = session.Database.ObjectFactory;
            var type = objectFactory.Assembly.GetType(objectFactory.Namespace + "." + objectType.PluralName);
            return (IObjects)Activator.CreateInstance(type, new object[] { session });
        }
    }
}