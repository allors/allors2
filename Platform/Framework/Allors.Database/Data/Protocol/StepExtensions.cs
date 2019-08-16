//-------------------------------------------------------------------------------------------------
// <copyright file="StepExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Protocol.Data
{
    using Allors.Meta;

    public static class StepExtensions
    {
        public static Allors.Data.Step Load(this Step @this, ISession session) =>
            new Allors.Data.Step
            {
                PropertyType = @this.PropertyType != null ? (IPropertyType)session.Database.ObjectFactory.MetaPopulation.Find(@this.PropertyType.Value) : null,
                Next = @this.Next?.Load(session),
                Include = @this.Include?.Load(session)
            };
    }
}
