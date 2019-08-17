// <copyright file="ProductAssociation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f194d2e1-d246-40eb-9eab-70ee2521703a")]
    #endregion
    public partial interface ProductAssociation : Commentable, Period, Object
    {
    }
}
