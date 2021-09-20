// <copyright file="Configuration.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using Meta;

    public class Configuration : Adapters.Configuration
    {
        public Configuration(string name, IMetaPopulation metaPopulation, ReflectionObjectFactory objectFactory) : base(name, metaPopulation, objectFactory) { }
    }
}
