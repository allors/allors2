// <copyright file="ObjectsBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using Allors.Domain;

    public abstract partial class ObjectsBase<T>
    {
        protected virtual void BasePrepare(Setup setup)
        {
        }

        protected virtual void BaseSetup(Setup setup)
        {
        }

        protected virtual void BaseSecure(Security config)
        {
        }
    }
}
