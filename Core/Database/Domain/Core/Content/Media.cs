// <copyright file="Media.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using DataUtils;
    using HeyRed.Mime;

    public partial class Media
    {
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
            this.FileName = $"{name}.{MimeTypesMap.GetExtension(this.Type)}";
        }

        public void CoreDelete(DeletableDelete method) => this.MediaContent?.Delete();
    }
}
