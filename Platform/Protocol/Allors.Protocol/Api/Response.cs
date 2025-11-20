// <copyright file="ErrorResponse.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote
{
    public abstract class Response
    {
        public bool HasErrors => this.versionErrors?.Length > 0 || this.accessErrors?.Length > 0 || this.missingErrors?.Length > 0 || this.derivationErrors?.Length > 0 || !string.IsNullOrWhiteSpace(this.errorMessage);

        /// <summary>
        /// Error Message
        /// </summary>
        public string errorMessage { get; set; }

        /// <summary>
        /// Version Errors
        /// </summary>
        public string[] versionErrors { get; set; }

        /// <summary>
        /// Access Errors
        /// </summary>
        public string[] accessErrors { get; set; }

        /// <summary>
        /// Missing Errors
        /// </summary>
        public string[] missingErrors { get; set; }

        /// <summary>
        /// Derivation Errors
        /// </summary>
        public ResponseDerivationError[] derivationErrors { get; set; }
    }
}
