// <copyright file="BarcodeType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    public enum BarcodeType
    {
        /// <summary>Aztec 2D barcode format.</summary>
        AZTEC = 1,

        /// <summary>CODABAR 1D format.</summary>
        CODABAR = 2,

        /// <summary>Code 39 1D format.</summary>
        CODE_39 = 4,

        /// <summary>Code 93 1D format.</summary>
        CODE_93 = 8,

        /// <summary>Code 128 1D format.</summary>
        CODE_128 = 16, // 0x00000010

        /// <summary>Data Matrix 2D barcode format.</summary>
        DATA_MATRIX = 32, // 0x00000020

        /// <summary>EAN-8 1D format.</summary>
        EAN_8 = 64, // 0x00000040

        /// <summary>EAN-13 1D format.</summary>
        EAN_13 = 128, // 0x00000080

        /// <summary>ITF (Interleaved Two of Five) 1D format.</summary>
        ITF = 256, // 0x00000100

        /// <summary>MaxiCode 2D barcode format.</summary>
        MAXICODE = 512, // 0x00000200

        /// <summary>PDF417 format.</summary>
        PDF_417 = 1024, // 0x00000400

        /// <summary>QR Code 2D barcode format.</summary>
        QR_CODE = 2048, // 0x00000800

        /// <summary>RSS 14</summary>
        RSS_14 = 4096, // 0x00001000

        /// <summary>RSS EXPANDED</summary>
        RSS_EXPANDED = 8192, // 0x00002000

        /// <summary>UPC-A 1D format.</summary>
        UPC_A = 16384, // 0x00004000

        /// <summary>UPC-E 1D format.</summary>
        UPC_E = 32768, // 0x00008000

        /// <summary>UPC/EAN extension format. Not a stand-alone format.</summary>
        UPC_EAN_EXTENSION = 65536, // 0x00010000

        /// <summary>MSI</summary>
        MSI = 131072, // 0x00020000

        /// <summary>Plessey</summary>
        PLESSEY = 262144, // 0x00040000

        /// <summary>Intelligent Mail barcode</summary>
        IMB = 524288, // 0x00080000
    }
}
