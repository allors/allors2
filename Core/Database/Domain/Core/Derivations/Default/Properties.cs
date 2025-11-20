// <copyright file="Properties.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Default
{
    using System.Collections.Generic;

    public class Properties
    {
        private Dictionary<string, object> properties;

        public object Get(string name)
        {
            var lowerName = name.ToLowerInvariant();

            if (this.properties != null && this.properties.TryGetValue(lowerName, out var value))
            {
                return value;
            }

            return null;
        }

        public void Set(string name, object value)
        {
            var lowerName = name.ToLowerInvariant();

            if (value == null)
            {
                if (this.properties != null)
                {
                    this.properties.Remove(lowerName);
                    if (this.properties.Count == 0)
                    {
                        this.properties = null;
                    }
                }
            }
            else
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string, object>();
                }

                this.properties[lowerName] = value;
            }
        }
    }
}
