//-------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseExtensions.cs" company="Allors bvba">
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
    public static partial class IDatabaseExtensions
    {
        private const string Key = nameof(IServiceLocator) + ".Key";

        public static IServiceLocator GetServiceLocator(this IDatabase @this)
        {
            return (IServiceLocator)@this[Key];
        }

        public static void SetServiceLocator(this IDatabase @this, IServiceLocator serviceLocator)
        {
            @this[Key] = serviceLocator;
        }
    }
}