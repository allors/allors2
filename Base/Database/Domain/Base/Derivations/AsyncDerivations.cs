// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncDerivations.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
    using System.Linq;

    public partial class AsyncDerivations
    {
        public static readonly string IsAsyncDerivationKey = $"{nameof(AsyncDerivations)}.IsAsyncDerivationKey";

        public static void AsyncDerive(IDatabase database)
        {
            using (var session = database.CreateSession())
            {
                try
                {
                    session[IsAsyncDerivationKey] = true;

                    var asyncDerivationsByAsyncDerivable =
                        session.Extent<AsyncDerivation>()
                            .GroupBy(v => v.AsyncDerivable)
                            .ToDictionary(g => g.Key, g => g.ToList());

                    var asyncDerivables = asyncDerivationsByAsyncDerivable.Keys.ToList();

                    foreach (var asyncDerivable in asyncDerivables)
                    {
                        var asyncDerivations = asyncDerivationsByAsyncDerivable[asyncDerivable];

                        try
                        {
                            asyncDerivable.AsyncDerive();

                            asyncDerivations.ForEach(asyncDerivation => asyncDerivation.Delete());
                            session.Commit();
                        }
                        catch
                        {
                            session.Rollback();

                            asyncDerivations.ForEach(asyncDerivation => asyncDerivation.Delete());
                            session.Commit();
                        }
                    }
                }
                finally
                {
                    session[IsAsyncDerivationKey] = false;
                }
            }
        }
    }
}