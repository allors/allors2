// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreparedPull.cs" company="Allors bvba">
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

using System.IO;

namespace Allors.Domain
{
    using System.Text;
    using System.Xml.Serialization;

    using Allors.Data;

    public partial class PreparedFetch
    {
        public Fetch Fetch
        {
            get
            {
                using (TextReader reader = new StringReader(this.Content))
                {
                    var protocolFetch = (Allors.Data.Protocol.Fetch)XmlSerializer.Deserialize(reader);
                    return protocolFetch.Load(this.strategy.Session);
                }
            }

            set
            {
                var stringBuilder = new StringBuilder();
                using (TextWriter writer = new StringWriter(stringBuilder))
                {
                    XmlSerializer.Serialize(writer, value.Save());
                    this.Content = stringBuilder.ToString();
                }
            }
        }

        private static XmlSerializer XmlSerializer => new XmlSerializer(typeof(Allors.Data.Protocol.Fetch));
    }
}