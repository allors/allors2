// <copyright file="Compressor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol
{
    using System.Collections.Generic;

    public class Decompressor
    {
        private readonly Dictionary<string, string> valueByKey;

        public Decompressor() => this.valueByKey = new Dictionary<string, string>();

        public string Read(string compressed, out bool first)
        {
            first = false;

            if (!string.IsNullOrEmpty(compressed))
            {
                if (compressed[0] == Compressor.IndexMarker)
                {
                    first = true;
                    var secondIndexMarkerIndex = compressed.IndexOf(Compressor.IndexMarker, 1);
                    var key = compressed.Substring(1, secondIndexMarkerIndex - 1);
                    var value = compressed.Substring(secondIndexMarkerIndex + 1);
                    this.valueByKey.Add(key, value);
                    return value;
                }

                return this.valueByKey[compressed];
            }

            return null;
        }
    }
}
