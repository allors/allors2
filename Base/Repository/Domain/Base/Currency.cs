// <copyright file="Currency.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    public partial class Currency : IUnitOfMeasure
    {
        #region inherited properties
        public string Abbreviation { get; set; }

        public LocalisedText[] LocalisedAbbreviations { get; set; }

        public string Description { get; set; }

        public UnitOfMeasureConversion[] UnitOfMeasureConversions { get; set; }

        public string Symbol { get; set; }
        #endregion
    }
}
