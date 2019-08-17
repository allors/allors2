// <copyright file="Arguments.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Arguments : IReadOnlyDictionary<string, object>
    {
        /// <summary>
        /// The key value pairs.
        /// </summary>
        private readonly IList<KeyValuePair<string, object>> keyValuePairs;

        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments"/> class.
        /// </summary>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        public Arguments(object arguments)
        {
            if (arguments is IEnumerable<KeyValuePair<string, object>> enumerable)
            {
                this.keyValuePairs = enumerable.ToList();
            }
            else
            {
                this.keyValuePairs = arguments.GetType().GetProperties()
                    .Select(v => new KeyValuePair<string, object>(v.Name, v.GetValue(arguments, null))).ToArray();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<string> Keys => this.keyValuePairs.Select(v => v.Key);

        /// <inheritdoc/>
        public IEnumerable<object> Values => this.keyValuePairs.Select(v => v.Value);

        /// <inheritdoc/>
        public int Count => this.keyValuePairs.Count;

        /// <inheritdoc/>
        public object this[string key] => this.keyValuePairs.FirstOrDefault(v => v.Key == key).Value;

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => this.keyValuePairs.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.keyValuePairs.GetEnumerator();

        /// <inheritdoc/>
        public bool ContainsKey(string key) => this.keyValuePairs.Any(v => v.Key.Equals(key));

        /// <inheritdoc/>
        public bool TryGetValue(string key, out object value)
        {
            var kvp = this.keyValuePairs.FirstOrDefault(v => v.Key == key);
            value = kvp.Key != null ? kvp.Value : null;
            return kvp.Key != null;
        }
    }
}
