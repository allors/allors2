// <copyright file="Population.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Domain;

namespace Tests
{
    using System.IO;

    using Allors;

    public class Population
    {
        private readonly ISession session;

        private readonly DirectoryInfo dataPath;

        public Population(ISession session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.dataPath = dataPath;
        }

        public void Execute()
        {
            var person = new PersonBuilder(this.session)
                .WithUserName("administrator")
                .Build();

            new UserGroups(this.session).Creators.AddMember(person);
            new UserGroups(this.session).Administrators.AddMember(person);

            this.session.Derive();
        }
    }
}
