// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bank.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Domain
{
    using System.Text.RegularExpressions;

    using Resources;

    public partial class Bank
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistBic)
            {
                if (!Regex.IsMatch(this.Bic, "^([a-zA-Z]){4}([a-zA-Z]){2}([0-9a-zA-Z]){2}([0-9a-zA-Z]{3})?$"))
                {
                    derivation.Log.AddError(this, Banks.Meta.Bic, ErrorMessages.NotAValidBic);
                }

                var country = new Countries(this.Strategy.Session).FindBy(Countries.Meta.IsoCode, this.Bic.Substring(4, 2));
                if (country == null)
                {
                    derivation.Log.AddError(this, Banks.Meta.Bic, ErrorMessages.NotAValidBic);
                }
            }
        }
    }
}