// <copyright file="ObjectState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("f991813f-3146-4431-96d0-554aa2186887")]
    #endregion
    public partial interface ObjectState : UniquelyIdentifiable
    {
        #region Allors
        [Id("913C994F-15B0-40D2-AC4F-81E362B9142C")]
        [AssociationId("EB17BE96-C9EA-4312-B358-47203F3062B5")]
        [RoleId("943036A4-9A1A-4C9B-9351-6E63FAA9FE9F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Restriction ObjectRestriction { get; set; }

        #region Allors
        [Id("b86f9e42-fe10-4302-ab7c-6c6c7d357c39")]
        [AssociationId("052ec640-3150-458a-99d5-0edce6eb6149")]
        [RoleId("945cbba6-4b09-4b87-931e-861b147c3823")]
        #endregion
        [Workspace]
        [Indexed]
        [Size(256)]
        string Name { get; set; }
    }
}
