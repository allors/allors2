// <copyright file="FromJsonVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using Allors.Protocol.Json;
    using Meta;
    using Pull = Allors.Protocol.Json.Data.Pull;
    using Select = Data.Select;

    public static class Extensions
    {
        public static IAssociationType FindAssociationType(this IMetaPopulation @this, string tag) => tag != null ? ((IRelationType)@this.FindByTag(tag)).AssociationType : null;

        public static IRoleType FindRoleType(this IMetaPopulation @this, string tag) => tag != null ? ((IRelationType)@this.FindByTag(tag)).RoleType : null;

        public static Data.Pull FromJson(this Pull pull, ITransaction transaction, IUnitConvert unitConvert)
        {
            var fromJsonVisitor = new FromJsonVisitor(transaction, unitConvert);
            pull.Accept(fromJsonVisitor);
            return fromJsonVisitor.Pull;
        }

        public static Data.IExtent FromJson(this Allors.Protocol.Json.Data.Extent extent, ITransaction transaction, IUnitConvert unitConvert)
        {
            var fromJsonVisitor = new FromJsonVisitor(transaction, unitConvert);
            extent.Accept(fromJsonVisitor);
            return fromJsonVisitor.Extent;
        }

        public static Select FromJson(this Allors.Protocol.Json.Data.Select @select, ITransaction transaction, IUnitConvert unitConvert)
        {
            var fromJsonVisitor = new FromJsonVisitor(transaction, unitConvert);
            @select.Accept(fromJsonVisitor);
            return fromJsonVisitor.Select;
        }

        public static Data.Procedure FromJson(this Allors.Protocol.Json.Data.Procedure procedure, ITransaction transaction, IUnitConvert unitConvert)
        {
            var fromJsonVisitor = new FromJsonVisitor(transaction, unitConvert);
            procedure.Accept(fromJsonVisitor);
            return fromJsonVisitor.Procedure;
        }

        public static Pull ToJson(this Data.Pull pull, IUnitConvert unitConvert)
        {
            var toJsonVisitor = new ToJsonVisitor(unitConvert);
            pull.Accept(toJsonVisitor);
            return toJsonVisitor.Pull;
        }

        public static Allors.Protocol.Json.Data.Extent ToJson(this Data.IExtent extent, IUnitConvert unitConvert)
        {
            var toJsonVisitor = new ToJsonVisitor(unitConvert);
            extent.Accept(toJsonVisitor);
            return toJsonVisitor.Extent;
        }

        public static Allors.Protocol.Json.Data.Select ToJson(this Select extent, IUnitConvert unitConvert)
        {
            var toJsonVisitor = new ToJsonVisitor(unitConvert);
            extent.Accept(toJsonVisitor);
            return toJsonVisitor.Select;
        }
    }
}
