// <copyright file="Template.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Document.OpenDocument;

    public partial class Template
    {
        public object Subject
        {
            get
            {
                var session = this.Strategy.Session;
                var caches = session.GetCache<TemplateCacheEntry>();
                caches.TryGetValue(this.Id, out var cache);
                if (cache == null || !this.Media.Revision.Equals(cache.Revision))
                {
                    cache = new TemplateCacheEntry(this, this.CreateSubject());
                    caches[this.Id] = cache;
                }

                return cache.Subject;
            }
        }

        public byte[] Render(object model, IDictionary<string, byte[]> images = null)
        {
            var properties = model.GetType().GetProperties();
            var dictionary = properties.ToDictionary(property => property.Name, property => property.GetValue(model));
            return this.Render(dictionary, images);
        }

        public byte[] Render(IDictionary<string, object> model, IDictionary<string, byte[]> images = null)
        {
            var subject = this.Subject;

            if (subject is OpenDocumentTemplate openDocumentTemplate)
            {
                return openDocumentTemplate.Render(model, images);
            }

            return null;
        }

        private object CreateSubject() =>
            this.TemplateType.IsOpenDocumentTemplate ?
                new OpenDocumentTemplate(this.Media.MediaContent.Data, this.Arguments) :
                null;
    }
}
