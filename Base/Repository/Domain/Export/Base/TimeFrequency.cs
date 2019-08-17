// <copyright file="TimeFrequency.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("1aba0c3c-2a1c-414d-86df-5a9b8c672587")]
    #endregion
    public partial class TimeFrequency : Enumeration, IUnitOfMeasure
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public string Abbreviation { get; set; }

        public LocalisedText[] LocalisedAbbreviations { get; set; }

        public string Description { get; set; }

        public UnitOfMeasureConversion[] UnitOfMeasureConversions { get; set; }

        public string Symbol { get; set; }

        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("BAE786CB-B99B-4BB7-8D0F-55BBF5392871")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
