// <copyright file="MediaContents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.IO;
    using HeyRed.Mime;

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

        public static string Sniff(byte[] content, string fileName)
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

            return !string.IsNullOrEmpty(fileName) ?
                MimeTypesMap.GetMimeType(fileName) :
                "application/octet-stream";
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
