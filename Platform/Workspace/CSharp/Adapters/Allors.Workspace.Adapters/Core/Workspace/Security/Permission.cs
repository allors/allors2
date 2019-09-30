// <copyright file="Context.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Domain;
    using Meta;

    public class Permission
    {
        public Permission(long id, IClass @class, IOperandType operandType, Operations operation)
        {
            this.Id = id;
            this.Class = @class;
            this.OperandType = operandType;
            this.Operation = operation;
        }

        public long Id { get; }

        public IClass Class { get; }

        public IOperandType OperandType { get; }

        public Operations Operation { get; }
    }
}
