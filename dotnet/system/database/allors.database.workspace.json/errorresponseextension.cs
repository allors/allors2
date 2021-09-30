// <copyright file="ErrorResponseExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api;
    using Derivations;

    public static class ResponseExtensions
    {
        public static void AddDerivationErrors(this Response @this, IValidation validation)
        {
            foreach (var derivationError in validation.Errors)
            {
                var derivationErrorResponse = new ResponseDerivationError
                {
                    m = derivationError.Message,
                    r = derivationError.Relations.Select(x => new DerivationRelation { i = x.Association.Id, r = x.RelationType.Tag }).ToArray(),
                };

                @this._d = @this._d != null ? new List<ResponseDerivationError>(@this._d) { derivationErrorResponse }.ToArray() : new List<ResponseDerivationError> { derivationErrorResponse }.ToArray();
            }
        }

        public static void AddVersionError(this Response @this, IObject obj) =>
            @this._v = @this._v != null ?
                new List<long>(@this._v) { obj.Id }.ToArray() :
                new List<long> { obj.Id }.ToArray();

        public static void AddAccessError(this Response @this, IObject obj) =>
            @this._a = @this._a != null ?
                new List<long>(@this._a) { obj.Id }.ToArray() :
                new List<long> { obj.Id }.ToArray();

        public static void AddMissingError(this Response @this, long id) =>
            @this._m = @this._m != null ?
                new List<long>(@this._m) { id }.ToArray() :
                new List<long> { id }.ToArray();
    }
}
