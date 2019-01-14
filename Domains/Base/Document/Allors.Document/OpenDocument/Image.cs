// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenDocumentRendering.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba
// This file is licenses under the Lesser General Public Licence v3 (LGPL)
// The LGPL License is included in the file LICENSE.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Document.OpenDocument
{
    public class Image
    {
        public string Name { get; set; }

        public byte[] Contents { get; set; }

        public string OriginalFullPath { get; set; }

        public string FullPath => $"Pictures/{this.Name}";
    }
}