//-------------------------------------------------------------------------------------------------
// <copyright file="TemplateBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the PermissionBuilder type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    using Allors.Document.OpenDocument;

    public partial class TemplateBuilder
    {
        public TemplateBuilder WithArguments<T>()
        {
            this.Arguments = OpenDocumentTemplate.InferArguments(typeof(T));
            return this;
        }

        public TemplateBuilder WithArguments(Type inferFromType)
        {
            this.Arguments = OpenDocumentTemplate.InferArguments(inferFromType);
            return this;
        }
    }
}
