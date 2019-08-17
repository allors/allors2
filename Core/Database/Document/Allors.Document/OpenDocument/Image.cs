// <copyright file="Image.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
