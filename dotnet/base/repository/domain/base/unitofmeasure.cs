// <copyright file="UnitOfMeasure.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5cd7ea86-8bc6-4b72-a8f6-788e6453acdc")]
    #endregion
    [Plural("UnitsOfMeasure")]
    public partial class UnitOfMeasure : IUnitOfMeasure
    {
        #region inherited properties

        public string Abbreviation { get; set; }

        public LocalisedText[] LocalisedAbbreviations { get; set; }

        public string Description { get; set; }

        public UnitOfMeasureConversion[] UnitOfMeasureConversions { get; set; }

        public string Symbol { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

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
        [Id("A10DE6C6-9011-43A1-990C-6A0EDDBA279E")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
