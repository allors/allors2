// <copyright file="BarcodeService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;
    using System.IO;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    using ZXing.Common;

    public class BarcodeService : IBarcodeService
    {
        public byte[] Generate(string content, BarcodeType type, int? width, int? height, int? margin)
        {
            ZXing.BarcodeFormat barcodeFormat;
            switch (type)
            {
                case BarcodeType.AZTEC:
                    barcodeFormat = ZXing.BarcodeFormat.AZTEC;
                    break;
                case BarcodeType.CODABAR:
                    barcodeFormat = ZXing.BarcodeFormat.CODABAR;
                    break;
                case BarcodeType.CODE_39:
                    barcodeFormat = ZXing.BarcodeFormat.CODE_39;
                    break;
                case BarcodeType.CODE_93:
                    barcodeFormat = ZXing.BarcodeFormat.CODE_93;
                    break;
                case BarcodeType.CODE_128:
                    barcodeFormat = ZXing.BarcodeFormat.CODE_128;
                    break;
                case BarcodeType.DATA_MATRIX:
                    barcodeFormat = ZXing.BarcodeFormat.DATA_MATRIX;
                    break;
                case BarcodeType.EAN_8:
                    barcodeFormat = ZXing.BarcodeFormat.EAN_8;
                    break;
                case BarcodeType.EAN_13:
                    barcodeFormat = ZXing.BarcodeFormat.EAN_13;
                    break;
                case BarcodeType.ITF:
                    barcodeFormat = ZXing.BarcodeFormat.ITF;
                    break;
                case BarcodeType.MAXICODE:
                    barcodeFormat = ZXing.BarcodeFormat.MAXICODE;
                    break;
                case BarcodeType.PDF_417:
                    barcodeFormat = ZXing.BarcodeFormat.PDF_417;
                    break;
                case BarcodeType.QR_CODE:
                    barcodeFormat = ZXing.BarcodeFormat.QR_CODE;
                    break;
                case BarcodeType.RSS_14:
                    barcodeFormat = ZXing.BarcodeFormat.RSS_14;
                    break;
                case BarcodeType.RSS_EXPANDED:
                    barcodeFormat = ZXing.BarcodeFormat.RSS_EXPANDED;
                    break;
                case BarcodeType.UPC_A:
                    barcodeFormat = ZXing.BarcodeFormat.UPC_A;
                    break;
                case BarcodeType.UPC_E:
                    barcodeFormat = ZXing.BarcodeFormat.UPC_E;
                    break;
                case BarcodeType.UPC_EAN_EXTENSION:
                    barcodeFormat = ZXing.BarcodeFormat.UPC_EAN_EXTENSION;
                    break;
                case BarcodeType.MSI:
                    barcodeFormat = ZXing.BarcodeFormat.MSI;
                    break;
                case BarcodeType.PLESSEY:
                    barcodeFormat = ZXing.BarcodeFormat.PLESSEY;
                    break;
                case BarcodeType.IMB:
                    barcodeFormat = ZXing.BarcodeFormat.IMB;
                    break;
                default:
                    throw new ArgumentException();
            }

            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = barcodeFormat
            };

            if (width.HasValue || height.HasValue || margin.HasValue)
            {
                barcodeWriter.Options = new EncodingOptions();
                if (width.HasValue)
                {
                    barcodeWriter.Options.Width = width.Value;
                }

                if (height.HasValue)
                {
                    barcodeWriter.Options.Height = height.Value;
                }

                if (margin.HasValue)
                {
                    barcodeWriter.Options.Margin = margin.Value;
                }
            }

            using (var image = barcodeWriter.Write(content))
            {
                using (var stream = new MemoryStream())
                {
                    image.SaveAsPng(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
