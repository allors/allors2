// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System.Collections.Generic;
    using System.IO;
    using Allors.Domain;
    using Allors.Meta;

    public partial class Setup
    {
        private readonly ISession session;

        private readonly Dictionary<IObjectType, IObjects> objectsByObjectType;
        private readonly ObjectsGraph objectsGraph;

        public Setup(ISession session, Config config)
        {
            this.Config = config;
            this.session = session;

            this.objectsByObjectType = new Dictionary<IObjectType, IObjects>();
            foreach (var objectType in session.Database.MetaPopulation.Composites)
            {
                this.objectsByObjectType[objectType] = objectType.GetObjects(session);
            }

            this.objectsGraph = new ObjectsGraph();
        }

        public Config Config { get; }

        public void Apply()
        {
            this.OnPrePrepare();

            foreach (var objects in this.objectsByObjectType.Values)
            {
                objects.Prepare(this);
            }

            this.OnPostPrepare();

            this.OnPreSetup();

            this.objectsGraph.Invoke(objects => objects.Setup(this));

            this.OnPostSetup();

            this.session.Derive();

            if (this.Config.SetupSecurity)
            {
                new Security(this.session).Apply();
            }
        }

        public void Add(IObjects objects) => this.objectsGraph.Add(objects);

        /// <summary>
        /// The dependee is set up before the dependent object;
        /// </summary>
        /// <param name="dependent"></param>
        /// <param name="dependee"></param>
        public void AddDependency(ObjectType dependent, ObjectType dependee) => this.objectsGraph.AddDependency(this.objectsByObjectType[dependent], this.objectsByObjectType[dependee]);

        private void CoreOnPrePrepare()
        {
        }

        private void CoreOnPostSetup()
        {
        }

        private void CoreOnPostPrepare()
        {
        }

        private void CoreOnPreSetup()
        {
        }
    }
}
