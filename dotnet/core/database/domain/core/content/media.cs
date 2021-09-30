// <copyright file="Media.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.IO;
    using System.Linq;
    using DataUtils;
    using HeyRed.Mime;

    public partial class Media
    {
        static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();
        static string[] InvalidFileNames = new[]
        {
            "CON", "PRN", "AUX", "NUL", "COM", "LPT"
        };

        public void CoreOnDerive(ObjectOnDerive method)
        {
            this.Revision = Guid.NewGuid();

            if (this.ExistInData || this.ExistInDataUri)
            {
                if (!this.ExistMediaContent)
                {
                    this.MediaContent = new MediaContentBuilder(this.Strategy.Session).Build();
                }
            }

            if (this.ExistInData)
            {
                this.MediaContent.Data = this.InData;
                this.MediaContent.Type = this.InType ?? MediaContents.Sniff(this.InData, this.InFileName);

                this.RemoveInType();
                this.RemoveInData();
            }

            if (this.ExistInFileName)
            {
                this.Name = Path.GetFileNameWithoutExtension(this.InFileName);
                this.RemoveInFileName();
            }

            if (this.ExistInDataUri)
            {
                var dataUrl = new DataUrl(this.InDataUri);

                this.MediaContent.Data = Convert.FromBase64String(dataUrl.ReadAsBase64EncodedString());
                this.MediaContent.Type = dataUrl.ContentType;

                this.RemoveInDataUri();
            }

            this.Type = this.MediaContent?.Type;

            var name = !string.IsNullOrWhiteSpace(this.Name) ? this.Name : this.UniqueId.ToString();
            var fileName = $"{name}.{MimeTypesMap.GetExtension(this.Type)}";
            var safeFileName = new string(fileName.Where(ch => !InvalidFileNameChars.Contains(ch)).ToArray());

            var uppercaseSafeFileName = safeFileName.ToUpperInvariant();
            foreach(var invalidFileName in InvalidFileNames)
            {
                if (uppercaseSafeFileName.StartsWith(invalidFileName))
                {
                    safeFileName += "_" + safeFileName;
                    break;
                }
            }

            this.FileName = safeFileName;
        }

        public void CoreDelete(DeletableDelete method) => this.MediaContent?.Delete();
    }
}
