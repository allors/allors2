// <copyright file="PermissionsWriter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json;
    using Data;

    public class Arguments : IArguments
    {
        private readonly IDictionary<string, object> arguments;
        private readonly IUnitConvert unitConvert;

        public Arguments(IDictionary<string, object> arguments, IUnitConvert unitConvert)
        {
            this.arguments = arguments;
            this.unitConvert = unitConvert;
        }

        public bool HasArgument(string name) => this.arguments.ContainsKey(name);

        public object ResolveUnit(string tag, string name) => this.unitConvert.FromJson(tag, this.arguments[name]);

        public object[] ResolveUnits(string tag, string name) => ((object[])this.arguments[name]).Select(v => this.unitConvert.FromJson(tag, v)).ToArray();

        public long ResolveObject(string name) => Convert.ToInt64(this.arguments[name]);

        public long[] ResolveObjects(string name) => ((object[])this.arguments[name]).Select(Convert.ToInt64).ToArray();

        public int ResolveMetaObject(string name) => Convert.ToInt32(this.arguments[name]);
    }
}
