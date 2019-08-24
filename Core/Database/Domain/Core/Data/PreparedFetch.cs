// <copyright file="PreparedFetch.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.IO;
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
