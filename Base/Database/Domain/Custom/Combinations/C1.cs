// --------------------------------------------------------------------------------------------------------------------
// <copyright file="C1.cs" company="Allors bvba">
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
    using global::System.Collections.Generic;

    using Allors;

    public partial class C1
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            foreach (Object dependency in this.Dependencies)
            {
                derivation.AddDependency(this, dependency);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var sequence = (IList<IObject>)derivation["sequence"];
            if (sequence != null)
            {
                sequence.Add(this);
            }
        }

        public void CustomSum(C1Sum method)
        {
            method.result = method.a + method.b;
        }

    }
}
