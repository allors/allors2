// <copyright file="Upgrade.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.IO;
    using System.Linq;

    public class Upgrade
    {
        private readonly ISession session;

        private readonly DirectoryInfo dataPath;

        public Upgrade(ISession session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.dataPath = dataPath;
        }

        public void Execute()
        {
        }

        private void Derive(Extent extent)
        {
            var derivation = new Domain.Derivations.Default.Derivation(this.session);
            derivation.Mark(extent.Cast<Domain.Object>().ToArray());
            var validation = derivation.Derive();
            if (validation.HasErrors)
            {
                foreach (var error in validation.Errors)
                {
                    Console.WriteLine(error.Message);
                }

                throw new Exception("Derivation Error");
            }
        }
    }
}
