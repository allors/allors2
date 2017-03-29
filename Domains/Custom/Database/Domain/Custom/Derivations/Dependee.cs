// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Dependee.cs" company="Allors bvba">
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
    public partial class Dependee
    {
        public void CustomOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCounter)
            {
                this.Counter = 0;
            }

            if (!this.ExistSubcounter)
            {
                this.Subcounter = 0;
            }

            if (!this.ExistDeleteDependent)
            {
                this.DeleteDependent = false;
            }
        }

        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            if (this.ExistDependentWhereDependee)
            {
                derivation.AddDependency(this.DependentWhereDependee, this);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            this.Counter = this.Counter + 1;

            if (this.ExistSubdependee)
            {
                this.Subcounter = this.Subdependee.Subcounter;
            }

            if (this.DeleteDependent.HasValue && this.DeleteDependent.Value)
            {
                this.DependentWhereDependee.Delete();
            }
        }
    }
}
