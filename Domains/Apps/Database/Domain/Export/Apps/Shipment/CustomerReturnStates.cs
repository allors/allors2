// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerReturnStates.cs" company="Allors bvba">
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

    public partial class CustomerReturnStates
    {
        public static readonly Guid ReceivedId = new Guid("32790FE5-69E3-46b1-BD23-D0A59D3B3794");

        private UniquelyIdentifiableSticky<CustomerReturnState> stateCache;

        public CustomerReturnState Received => this.StateCache[ReceivedId];

        private UniquelyIdentifiableSticky<CustomerReturnState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<CustomerReturnState>(this.Session));
    }
}