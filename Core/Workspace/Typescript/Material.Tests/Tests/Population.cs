// <copyright file="Population.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests
{
    using System.IO;

    using Allors;

    public class Population
    {
        private readonly ISession session;

        private DirectoryInfo dataPath;

        public Population(ISession session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.dataPath = dataPath;
        }

        public void Execute() => this.session.Derive();
    }
}
