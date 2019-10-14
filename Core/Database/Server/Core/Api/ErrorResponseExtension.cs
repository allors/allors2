// <copyright file="ErrorResponseExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Domain;
    using Allors.Protocol.Remote;
    using Protocol;

    public static class ErrorResponseExtensions
    {
        public static void AddDerivationErrors(this ErrorResponse @this, IValidation validation, MetaObjectCompressor metaObjectCompressor)
        {
            foreach (var derivationError in validation.Errors)
            {
                var derivationErrorResponse = new DerivationErrorResponse
                {
                    M = derivationError.Message,
                    R = derivationError.Relations.Select(x => new[] { x.Association.Id.ToString(), metaObjectCompressor.Write(x.RoleType) }).ToArray(),
                };

                @this.DerivationErrors = @this.DerivationErrors != null ?
                                             new List<DerivationErrorResponse>(@this.DerivationErrors) { derivationErrorResponse }.ToArray() :
                                             new List<DerivationErrorResponse> { derivationErrorResponse }.ToArray();
            }
        }

        public static void AddVersionError(this ErrorResponse @this, IObject obj) =>
            @this.VersionErrors = @this.VersionErrors != null ?
                new List<string>(@this.VersionErrors) { obj.Id.ToString() }.ToArray() :
                new List<string> { obj.Id.ToString() }.ToArray();

        public static void AddAccessError(this ErrorResponse @this, IObject obj) =>
            @this.AccessErrors = @this.AccessErrors != null ?
                new List<string>(@this.AccessErrors) { obj.Id.ToString() }.ToArray() :
                new List<string> { obj.Id.ToString() }.ToArray();

        public static void AddMissingError(this ErrorResponse @this, string id) =>
            @this.MissingErrors = @this.MissingErrors != null ?
                new List<string>(@this.MissingErrors) { id }.ToArray() :
                new List<string> { id }.ToArray();
    }
}
