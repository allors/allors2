// -------------------------------------------------------------------------------------------------
// <copyright file="ResourceFile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Log type.</summary>
// ---------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Resources
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;

    internal class ResourceFile
    {
        private readonly Dictionary<string, object> dictionary;

        internal ResourceFile(FileInfo fileInfo)
        {
            this.dictionary = new Dictionary<string, object>();

            using (var resxReader = new ResourceReader(fileInfo.FullName))
            {
                foreach (DictionaryEntry entry in resxReader)
                {
                    this.dictionary.Add((string)entry.Key, entry.Value);
                }
            }
        }

        public void Merge(Dictionary<string, object> mergedDictionary)
        {
            foreach (var entry in this.dictionary)
            {
                mergedDictionary[entry.Key] = entry.Value;
            }
        }
    }
}
