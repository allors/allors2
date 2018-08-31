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
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Domain;
    using Allors.Server.Protocol;

    public static class ErrorResponseExtensions
    {
        public static void AddDerivationErrors(this ErrorResponse @this, IValidation validation)
        {
            foreach (var derivationError in validation.Errors)
            {
                var derivationErrorResponse = new DerivationErrorResponse
                                                  {
                                                      M = derivationError.Message,  
                                                      R = derivationError.Relations.Select(x => new[] { x.Association.Id.ToString(), x.RoleType.Name }).ToArray()
                                                  };

                @this.DerivationErrors = @this.DerivationErrors != null ? 
                                             new List<DerivationErrorResponse>(@this.DerivationErrors) { derivationErrorResponse }.ToArray() : 
                                             new List<DerivationErrorResponse> { derivationErrorResponse }.ToArray();
            }
        }

        public static void AddVersionError(this ErrorResponse @this, IObject obj)
        {
            @this.VersionErrors = @this.VersionErrors != null ? 
                                      new List<string>(@this.VersionErrors) { obj.Id.ToString() }.ToArray() : 
                                      new List<string> { obj.Id.ToString() }.ToArray();
        }

        public static void AddAccessError(this ErrorResponse @this, IObject obj)
        {
            @this.AccessErrors = @this.AccessErrors != null ? 
                                     new List<string>(@this.AccessErrors) { obj.Id.ToString() }.ToArray() : 
                                     new List<string> { obj.Id.ToString() }.ToArray();
        }

        public static void AddMissingError(this ErrorResponse @this, string id)
        {
            @this.MissingErrors = @this.MissingErrors != null ?
                                      new List<string>(@this.MissingErrors) { id }.ToArray() :
                                      new List<string> { id }.ToArray();
        }
    }
}