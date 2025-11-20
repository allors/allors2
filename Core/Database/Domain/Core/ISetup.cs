// <copyright file="ISetup.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    public interface ISetup
    {
        void Prepare(Setup setup);

        void Setup(Setup setup);
    }
}
