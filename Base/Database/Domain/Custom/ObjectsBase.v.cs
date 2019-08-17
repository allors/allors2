// <copyright file="ObjectsBase.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using Allors.Domain;

    public abstract partial class ObjectsBase<T> where T : IObject
    {
        public void Prepare(Setup setup)
        {
            this.CorePrepare(setup);
            this.BasePrepare(setup);
            this.CustomPrepare(setup);
        }

        public void Setup(Setup setup)
        {
            this.CoreSetup(setup);
            this.BaseSetup(setup);
            this.CustomSetup(setup);
        }

        public void Secure(Security security)
        {
            this.CoreSecure(security);
            this.BaseSecure(security);
            this.CustomSecure(security);
        }
    }
}
