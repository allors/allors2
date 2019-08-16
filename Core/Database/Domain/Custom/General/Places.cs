
// <copyright file="Places.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Places
    {
        public Extent<Place> ExtentByPostalCode()
        {
            var places = this.Session.Extent<Place>();
            places.AddSort(M.Place.PostalCode);

            return places;
        }
    }
}
