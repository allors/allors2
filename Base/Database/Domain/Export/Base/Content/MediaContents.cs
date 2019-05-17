// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaContents.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    using System.Collections.Generic;

    public partial class MediaContents
    {
        // File signatures
        // See http://en.wikipedia.org/wiki/List_of_file_signatures and http://www.garykessler.net/library/file_sigs.html
        private static readonly byte[] PngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] Jpeg2000Signature = { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20, 0x0D, 0x0A };
        private static readonly byte[] JpegSignature = { 0xFF, 0xD8, 0xFF };
        private static readonly byte[] Gif87ASignature = { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };
        private static readonly byte[] Gif89ASignature = { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };
        private static readonly byte[] BmpSignature = { 0x42, 0x4D };
        private static readonly byte[] PdfSignature = { 0x25, 0x50, 0x44, 0x46 };

        public static string Sniff(byte[] content)
        {
            if (Match(content, PngSignature))
            {
                return "image/png";
            }

            if (Match(content, Jpeg2000Signature) || Match(content, JpegSignature))
            {
                return "image/jpeg";
            }

            if (Match(content, Gif87ASignature) || Match(content, Gif89ASignature))
            {
                return "image/gif";
            }

            if (Match(content, BmpSignature))
            {
                return "image/bmp";
            }

            if (Match(content, PdfSignature))
            {
                return "application/pdf";
            }

            return "application/octet-stream";
        }

        protected override void BaseSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static bool Match(IReadOnlyList<byte> content, IReadOnlyList<byte> signature)
        {
            if (content.Count > signature.Count)
            {
                for (var i = 0; i < signature.Count; i++)
                {
                    if (content[i] != signature[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}