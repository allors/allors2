// <copyright file="ErrorResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api
{
    public abstract class Response
    {
        public bool HasErrors => this._v?.Length > 0 || this._a?.Length > 0 || this._m?.Length > 0 || this._d?.Length > 0 || !string.IsNullOrWhiteSpace(this._e);

        /// <summary>
        /// ErrorMessage
        /// </summary>
        public string _e { get; set; }

        /// <summary>
        /// VersionErrors
        /// </summary>
        public long[] _v { get; set; }

        /// <summary>
        /// AccessErrors
        /// </summary>
        public long[] _a { get; set; }

        /// <summary>
        /// MissingErrors
        /// </summary>
        public long[] _m { get; set; }

        /// <summary>
        /// DerivationErrors
        /// </summary>
        public ResponseDerivationError[] _d { get; set; }
    }
}
