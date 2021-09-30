// <copyright file="S1.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("253b0d71-9eaa-4d87-9094-3b549d8446b3")]
    #endregion
    public partial interface S1 : Object
    {
        [Id("2E52966D-6760-45A0-B687-0A0B6198A770")]
        void SuperinterfaceMethod();
    }
}
