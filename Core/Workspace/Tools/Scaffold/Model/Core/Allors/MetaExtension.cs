// <copyright file="MetaExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public partial class MetaExtension
    {
        public Guid Id { get; set; }

        public string List { get; set; }

        public string Overview { get; set; }

        public string Create { get; set; }

        public string Edit { get; set; }

        public static void Load(Dictionary<Guid, MetaExtension> metaExtensions, JArray jsonMetaExtensions, Action<MetaExtension, JToken> setter)
        {
            foreach (var json in jsonMetaExtensions)
            {
                if (json["id"] != null)
                {
                    Guid.TryParse(json["id"].Value<string>(), out var id);
                    if (!metaExtensions.TryGetValue(id, out var metaExtension))
                    {
                        metaExtension = new MetaExtension
                        {
                            Id = id,
                        };
                        metaExtensions.Add(id, metaExtension);
                    }

                    setter(metaExtension, json);
                }
            }
        }
    }
}
