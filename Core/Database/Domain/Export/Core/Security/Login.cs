
// <copyright file="Login.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Login
    {
        public void CoreOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistUser)
            {
                this.Delete();
            }
        }
    }
}
