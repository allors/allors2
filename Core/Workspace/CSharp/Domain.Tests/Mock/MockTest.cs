// <copyright file="LocalTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Mock
{
    using System;

    using Allors.Workspace;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;

    public class MockTest : IDisposable
    {
        public Workspace Workspace { get; set; }

        public MockTest()
        {
            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
            this.Workspace = new Workspace(objectFactory);
        }

        public void Dispose()
        {
        }
    }
}
