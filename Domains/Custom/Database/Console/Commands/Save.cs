// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Populate.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;
    using System.IO;
    using System.Xml;

    public class Save : Command
    {
        private readonly FileInfo fileInfo;

        public Save(string file)
        {
            this.fileInfo = new FileInfo(file);
        }

        public override void Execute()
        {
            var database = this.CreateDatabase();

            using (var stream = File.Create(this.fileInfo.FullName))
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    Console.WriteLine("Saving to " + this.fileInfo.FullName);
                    database.Save(writer);
                    Console.WriteLine("Saved to " + this.fileInfo.FullName);
                }
            }
        }
    }
}