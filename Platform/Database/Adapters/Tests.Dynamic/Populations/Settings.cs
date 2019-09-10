// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System;

    public static class Settings
    {
        // Smoke
        private const int DefaultNumberOfRuns = 2;
        private const int DefaultLargeArraySize = 10;
        private const bool DefaultExtraMarkers = true;
        private const bool DefaultExtraInits = true;

        // Full
        //private const int DefaultNumberOfRuns = 2;
        //private const int DefaultLargeArraySize = 1000;
        //private const bool DefaultExtraMarkers = true;
        //private const bool DefaultExtraInits = true;

        static Settings()
        {
            int numberOfRuns;
            NumberOfRuns = int.TryParse(Environment.GetEnvironmentVariable("NumberOfRuns"), out numberOfRuns) ? numberOfRuns : DefaultNumberOfRuns;

            int largeArraySize;
            LargeArraySize = int.TryParse(Environment.GetEnvironmentVariable("LargeArraySize"), out largeArraySize) ? largeArraySize : DefaultLargeArraySize;
            
            bool extraMarkers;
            ExtraMarkers = bool.TryParse(Environment.GetEnvironmentVariable("ExtraMarkers"), out extraMarkers) ? extraMarkers : DefaultExtraMarkers;

            bool extraInits;
            ExtraInits = bool.TryParse(Environment.GetEnvironmentVariable("ExtraCaches"), out extraInits) ? extraInits : DefaultExtraInits;
        }

        public static int NumberOfRuns { get; set; }

        public static int LargeArraySize { get; set; }

        public static bool ExtraMarkers { get; set; }

        public static bool ExtraInits { get; set; }
    }
}