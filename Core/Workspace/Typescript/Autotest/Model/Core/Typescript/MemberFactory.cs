// <copyright file="MemberFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Typescript
{
    using System;

    using Newtonsoft.Json.Linq;

    public static class MemberFactory
    {
        public static IMember Create(JToken json)
        {
            var kind = json["kind"]?.Value<string>();
            switch (kind)
            {
                case "property":
                    return new Property(json);

                case "method":
                    return new Method(json);

                default:
                    throw new Exception($"Unknown member: {kind}");
            }
        }
    }
}
