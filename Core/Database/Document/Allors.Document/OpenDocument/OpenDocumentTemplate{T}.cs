// <copyright file="OpenDocumentTemplate{T}.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document.OpenDocument
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class OpenDocumentTemplate<T> : OpenDocumentTemplate
    {
        private readonly PropertyInfo[] properties;

        public OpenDocumentTemplate(byte[] document, char leftDelimiter = DefaultLeftDelimiter, char rightDelimiter = DefaultRightDelimiter)
            : base(document, OpenDocumentTemplate.InferArguments(typeof(T)), leftDelimiter, rightDelimiter) =>
            this.properties = typeof(T).GetProperties();

        public byte[] Render(T model, Dictionary<string, byte[]> imageByName = null)
        {
            var dictionary = this.properties.ToDictionary(property => property.Name, property => property.GetValue(model));
            return this.Render(dictionary, imageByName);
        }
    }
}
