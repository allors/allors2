// <copyright file="Countries.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

    public partial class Countries
    {
        public static List<string> EuMemberStates = new List<string>
            {
                "BE", "BG", "CZ", "DK", "DE", "EE", "IE", "GR", "ES",
                "FR", "IT", "CY", "LV", "LT", "LU", "HU", "MT", "NL",
                "AT", "PL", "PT", "RO", "SI", "SK", "FI", "SE", "GB",
            };

        public static Dictionary<string, IbanData> IbanDataByCountry = new Dictionary<string, IbanData>
            {
                { "AD", new IbanData(24, @"\d{8}[a-zA-Z0-9]{12}") },
                { "AL", new IbanData(28, @"\d{8}[a-zA-Z0-9]{16}") },
                { "AT", new IbanData(20, @"\d{16}") },
                { "BA", new IbanData(20, @"\d{16}") },
                { "BE", new IbanData(16, @"\d{12}") },
                { "BG", new IbanData(22, @"[A-Z]{4}\d{6}[a-zA-Z0-9]{8}") },
                { "CH", new IbanData(21, @"\d{5}[a-zA-Z0-9]{12}") },
                { "CY", new IbanData(28, @"\d{8}[a-zA-Z0-9]{16}") },
                { "CZ", new IbanData(24, @"\d{20}") },
                { "DE", new IbanData(22, @"\d{18}") },
                { "DK", new IbanData(18, @"\d{14}") },
                { "EE", new IbanData(20, @"\d{16}") },
                { "ES", new IbanData(24, @"\d{20}") },
                { "FI", new IbanData(18, @"\d{14}") },
                { "FO", new IbanData(18, @"\d{14}") },
                { "FR", new IbanData(27, @"\d{10}[a-zA-Z0-9]{11}\d\d") },
                { "GB", new IbanData(22, @"[A-Z]{4}\d{14}") },
                { "GI", new IbanData(23, @"[A-Z]{4}[a-zA-Z0-9]{15}") },
                { "DL", new IbanData(18, @"\d{14}") },
                { "GR", new IbanData(27, @"\d{7}[a-zA-Z0-9]{16}") },
                { "HR", new IbanData(21, @"\d{17}") },
                { "HU", new IbanData(28, @"\d{24}") },
                { "IE", new IbanData(22, @"[A-Z]{4}\d{14}") },
                { "IL", new IbanData(23, @"\d{19}") },
                { "IS", new IbanData(26, @"\d{22}") },
                { "IT", new IbanData(27, @"[A-Z]\d{10}[a-zA-Z0-9]{12}") },
                { "LB", new IbanData(28, @"\d{4}[a-zA-Z0-9]{20}") },
                { "LI", new IbanData(21, @"\d{5}[a-zA-Z0-9]{12}") },
                { "LT", new IbanData(20, @"\d{16}") },
                { "LU", new IbanData(20, @"\d{3}[a-zA-Z0-9]{13}") },
                { "LV", new IbanData(21, @"[A-Z]{4}[a-zA-Z0-9]{13}") },
                { "MC", new IbanData(27, @"\d{10}[a-zA-Z0-9]{11}\d\d") },
                { "ME", new IbanData(22, @"\d{18}") },
                { "MK", new IbanData(19, @"\d{3}[a-zA-Z0-9]{10}\d\d") },
                { "MT", new IbanData(31, @"[A-Z]{4}\d{5}[a-zA-Z0-9]{18}") },
                { "MU", new IbanData(30, @"[A-Z]{4}\d{19}[A-Z]{3}") },
                { "NL", new IbanData(18, @"[A-Z]{4}\d{10}") },
                { "NO", new IbanData(15, @"\d{11}") },
                { "PL", new IbanData(28, @"\d{8}[a-zA-Z0-9]{16}") },
                { "PT", new IbanData(25, @"\d{21}") },
                { "RO", new IbanData(24, @"[A-Z]{4}[a-zA-Z0-9]{16}") },
                { "RS", new IbanData(22, @"\d{18}") },
                { "SA", new IbanData(24, @"\d{2}[a-zA-Z0-9]{18}") },
                { "SE", new IbanData(24, @"\d{20}") },
                { "SI", new IbanData(19, @"\d{15}") },
                { "SK", new IbanData(24, @"\d{20}") },
                { "SM", new IbanData(27, @"[A-Z]\d{10}[a-zA-Z0-9]{12}") },
                { "TN", new IbanData(24, @"\d{20}") },
                { "TR", new IbanData(26, @"\d{5}[a-zA-Z0-9]{17}") },
            };
    }

    public class IbanData
    {
        public int Length;
        public string RegexStructure;

        public IbanData()
        {
        }

        public IbanData(int length, string regexStructure)
            : this()
        {
            this.Length = length;
            this.RegexStructure = regexStructure;
        }
    }
}
