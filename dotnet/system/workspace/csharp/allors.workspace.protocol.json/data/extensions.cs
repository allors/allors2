// <copyright file="FromJsonVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Protocol.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json;
    using Procedure = Allors.Protocol.Json.Data.Procedure;
    using Pull = Allors.Protocol.Json.Data.Pull;

    public static class Extensions
    {
        public static Pull ToJson(this Data.Pull pull, IUnitConvert unitConvert)
        {
            var toJsonVisitor = new ToJsonVisitor(unitConvert);
            pull.Accept(toJsonVisitor);
            return toJsonVisitor.Pull;
        }

        public static Procedure ToJson(this Data.Procedure procedure, IUnitConvert unitConvert)
        {
            var toJsonVisitor = new ToJsonVisitor(unitConvert);
            procedure.Accept(toJsonVisitor);
            return toJsonVisitor.Procedure;
        }

        public static string[][] ToJsonForCollectionByName(this IDictionary<string, IObject[]> collectionByName) =>
            collectionByName?.Select(kvp =>
            {
                var name = kvp.Key;
                var collection = kvp.Value;

                if (collection == null || collection.Length == 0)
                {
                    return new[] { name };
                }

                var jsonCollection = new string[collection.Length + 1];
                jsonCollection[0] = name;

                for (var i = 0; i < collection.Length; i++)
                {
                    jsonCollection[i + 1] = collection[i].Id.ToString();
                }

                return jsonCollection;
            }).ToArray();

        public static string[][] ToJsonForObjectByName(this IDictionary<string, IObject> objectByName) =>
            objectByName?.Select(kvp =>
            {
                var name = kvp.Key;
                var @object = kvp.Value;

                return @object == null ? new[] { name } : new[] { name, @object.Id.ToString() };
            }).ToArray();

        public static string[][] ToJsonForVersionByObject(this IDictionary<IObject, long> versionByObject) =>
            versionByObject?.Select(kvp => new[] { kvp.Key.Id.ToString(), kvp.Value.ToString() }).ToArray();
    }
}
