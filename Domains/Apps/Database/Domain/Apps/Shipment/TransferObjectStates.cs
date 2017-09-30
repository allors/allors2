// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferStates.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class TransferStates
    {
        public static readonly Guid CreatedId = new Guid("ADAD2959-5472-4aed-977B-C04FBC67FAD8");

        private UniquelyIdentifiableCache<TransferState> stateCache;

        public TransferState Created => this.StateCache.Get(CreatedId);

        private UniquelyIdentifiableCache<TransferState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<TransferState>(this.Session));
    }
}