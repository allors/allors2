// <copyright file="C1.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

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

        public void CustomSum(C1Sum method) => method.Result = method.A + method.B;

        public override string ToString() => this.Name;
    }
}
