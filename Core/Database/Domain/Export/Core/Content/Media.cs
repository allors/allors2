// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Media.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using System.Text.RegularExpressions;

    public partial class Media
    {
        public string TypeExtension => this.Type?.Split('/').LastOrDefault();

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
                this.MediaContent.Type = MediaContents.Sniff(this.InData);

                this.RemoveInData();
            }

            if (this.ExistInDataUri)
            {
                var regex = new Regex(@"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)", RegexOptions.Compiled);

                var match = regex.Match(this.InDataUri);

                var mime = match.Groups["mime"].Value;
                var encoding = match.Groups["encoding"].Value;
                var data = match.Groups["data"].Value;

                var binaryData = Convert.FromBase64String(data);

                this.MediaContent.Data = binaryData;
                this.MediaContent.Type = mime;

                this.RemoveInDataUri();
            }
        }

        public void CoreOnPostDerive(ObjectOnPostDerive method)
        {
            this.Type = this.MediaContent?.Type;
        }

        public void CoreDelete(DeletableDelete method)
        {
            this.MediaContent?.Delete();
        }
    }
}
