// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Subdependee.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
    public partial class Subdependee
    {
        public void CustomOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSubcounter)
            {
                this.Subcounter = 0;
            }
        }

        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            if (this.ExistDependeeWhereSubdependee)
            {
                derivation.AddDependency(this.DependeeWhereSubdependee, this);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            this.Subcounter = this.Subcounter + 1;
        }
    }
}
