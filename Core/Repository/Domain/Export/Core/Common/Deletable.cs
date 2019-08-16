//-------------------------------------------------------------------------------------------------
// <copyright file="Deletable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("9279e337-c658-4086-946d-03c75cdb1ad3")]
    #endregion
    public partial interface Deletable : Object
    {
        [Id("430702D2-E02B-45AD-9B22-B8331DC75A3F")]
        [Workspace]
        void Delete();
    }
}
