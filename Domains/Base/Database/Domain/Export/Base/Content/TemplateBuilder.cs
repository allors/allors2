//-------------------------------------------------------------------------------------------------
// <copyright file="PermissionBuilder.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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