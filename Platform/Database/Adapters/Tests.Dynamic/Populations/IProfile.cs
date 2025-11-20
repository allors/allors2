// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProfile.cs" company="Allors bv">
//   Copyright Allors bv.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters
{
    using Allors;
    using System;

    public interface IProfile : IDisposable
    {
        IDatabase Database { get; }

        ISession Session { get; }

        Action[] Markers { get; }

        Action[] Inits { get; }

        IObjectFactory ObjectFactory { get; }

        IDatabase CreateDatabase();
    }
}