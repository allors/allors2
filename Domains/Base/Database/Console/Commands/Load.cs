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
    using System.Data;
    using System.IO;
    using System.Xml;

    public class Load : Command
    {
        private readonly FileInfo fileInfo;

        public Load(string file)
        {
            this.fileInfo = new FileInfo(file);
        }

        public override void Execute()
        {
            var database = this.CreateDatabase(IsolationLevel.Serializable);

            using (var reader = XmlReader.Create(this.fileInfo.FullName))
            {
                Console.WriteLine("Loading from " + this.fileInfo.FullName);
                database.Load(reader);
                Console.WriteLine("Loaded from " + this.fileInfo.FullName);
            }
        }
    }
}