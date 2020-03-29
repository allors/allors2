// <copyright file="SerialisedItemExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using Allors.Domain;
 
    public static partial class SerialisedItemExtensions
    {
        public static string DisplayName(this SerialisedItem @this) => @this.ItemNumber + " " + @this.Name + " SN: " + @this.SerialNumber;
    }
}
