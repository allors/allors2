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

namespace Allors.Server.Protocol
{
    public abstract class ErrorResponse
    {
        public bool HasErrors => this.VersionErrors?.Length > 0 || this.AccessErrors?.Length > 0 || this.MissingErrors?.Length > 0 || this.DerivationErrors?.Length > 0 || !string.IsNullOrWhiteSpace(this.ErrorMessage);

        public string ErrorMessage { get; set; }

        public string[] VersionErrors { get; set; }

        public string[] AccessErrors { get; set; }

        public string[] MissingErrors { get; set; }

        public DerivationErrorResponse[] DerivationErrors { get; set; }
    }
}