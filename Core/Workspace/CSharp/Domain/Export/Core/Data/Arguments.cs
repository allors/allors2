//------------------------------------------------------------------------------------------------- 
// <copyright file="Arguments.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Data
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
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.keyValuePairs.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.keyValuePairs.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            return this.keyValuePairs.Any(v => v.Key.Equals(key));
        }

        /// <inheritdoc/>
        public bool TryGetValue(string key, out object value)
        {
            var kvp = this.keyValuePairs.FirstOrDefault(v => v.Key == key);
            value = kvp.Key != null ? kvp.Value : null;
            return kvp.Key != null;
        }
    }
}
