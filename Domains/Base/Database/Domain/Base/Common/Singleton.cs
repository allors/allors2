// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Singleton.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors;

    public partial class Singleton
    {
        private const string SessionKey = nameof(Singleton) + ".Key";

        public static Singleton Instance(ISession session)
        {
            var instance = (Singleton)session[SessionKey] ?? session.Extent<Singleton>().First;

            session[SessionKey] = instance;
            return instance;
        }
    }
}