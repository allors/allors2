// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Template.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Document.OpenDocument;

    public partial class Template
    {
        public byte[] Render(object model, IDictionary<string, byte[]> images = null)
        {
            var properties = model.GetType().GetProperties();
            var dictionary = properties.ToDictionary(property => property.Name, property => property.GetValue(model));
            return this.Render(dictionary, images);
        }

        public byte[] Render(IDictionary<string, object> model, IDictionary<string, byte[]> images = null)
        {
            var template = new OpenDocumentTemplate(this.Media.MediaContent.Data);
            return template.Render(model, images);
        }
    }
}