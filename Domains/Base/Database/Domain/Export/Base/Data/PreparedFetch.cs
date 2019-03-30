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

    using Allors.Protocol.Data;

    public partial class PreparedFetch
    {
        public Allors.Data.Fetch Fetch
        {
            get
            {
                using (TextReader reader = new StringReader(this.Content))
                {
                    var protocolFetch = (Fetch)XmlSerializer.Deserialize(reader);
                    return protocolFetch.Load(this.Strategy.Session);
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

        private static XmlSerializer XmlSerializer => new XmlSerializer(typeof(Fetch));
    }
}