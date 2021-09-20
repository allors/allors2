// <copyright file="Cities.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Cities
    {
        private Sticky<string, City> cityByName;

        public Sticky<string, City> CityByName => this.cityByName ??= new Sticky<string, City>(this.Session, M.City.Name);
    }
}
