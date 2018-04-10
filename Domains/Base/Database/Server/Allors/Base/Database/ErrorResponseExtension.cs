// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorResponse.cs" company="Allors bvba">
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

namespace Allors.Server
{
    using System.Linq;

    using Allors.Domain;

    public static class ErrorResponseExtensions
    {
        public static void AddDerivationErrors(this ErrorResponse @this, IValidation validation)
        {
            foreach (IDerivationError derivationError in validation.Errors)
            {
                @this.DerivationErrors.Add(new DerivationErrorResponse
                                              {
                                                  M = derivationError.Message,  
                                                  R = derivationError.Relations.Select(x => new string[] { x.Association.Id.ToString(), x.RoleType.Name }).ToArray()
                                              });
            }
        }
    }
}