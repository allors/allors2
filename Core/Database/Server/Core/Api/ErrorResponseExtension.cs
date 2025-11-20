// <copyright file="ErrorResponseExtension.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Remote;
    using Domain.Derivations;

    public static class ErrorResponseExtensions
    {
        public static void AddDerivationErrors(this Response @this, IValidation validation)
        {
            foreach (var derivationError in validation.Errors)
            {
                var derivationErrorResponse = new ResponseDerivationError
                {
                    m = derivationError.Message,
                    r = derivationError.Relations.Select(x => new[] { x.Association?.Id.ToString(), x.RoleType?.IdAsString }).ToArray(),
                };

                @this.derivationErrors = @this.derivationErrors != null ?
                                             new List<ResponseDerivationError>(@this.derivationErrors) { derivationErrorResponse }.ToArray() :
                                             new List<ResponseDerivationError> { derivationErrorResponse }.ToArray();
            }
        }

        public static void AddVersionError(this Response @this, IObject obj) =>
            @this.versionErrors = @this.versionErrors != null ?
                new List<string>(@this.versionErrors) { obj.Id.ToString() }.ToArray() :
                new List<string> { obj.Id.ToString() }.ToArray();

        public static void AddAccessError(this Response @this, IObject obj) =>
            @this.accessErrors = @this.accessErrors != null ?
                new List<string>(@this.accessErrors) { obj.Id.ToString() }.ToArray() :
                new List<string> { obj.Id.ToString() }.ToArray();

        public static void AddMissingError(this Response @this, string id) =>
            @this.missingErrors = @this.missingErrors != null ?
                new List<string>(@this.missingErrors) { id }.ToArray() :
                new List<string> { id }.ToArray();
    }
}
