// <copyright file="Compressor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol
{
    using System.Collections.Generic;

    public class Compressor
    {
        public const char IndexMarker = '~';

        public const char ItemSeparator = ',';

        private readonly Dictionary<string, string> keyByValue;

        private int counter;

        public Compressor()
        {
            this.keyByValue = new Dictionary<string, string>();
            this.counter = 0;
        }

        public string Write(string value)
        {
            if (value == null)
            {
                return null;
            }

            if (this.keyByValue.TryGetValue(value, out var key))
            {
                return key;
            }

            key = (++this.counter).ToString();
            this.keyByValue.Add(value, key);
            return $"{IndexMarker}{key}{IndexMarker}{value}";
        }
    }
}
