// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    public interface IVisitor
    {
        void VisitExtent(Extent visited);

        void VisitSelect(Select @select);

        void VisitNode(Node node);

        void VisitPredicate(Predicate predicate);

        void VisitPull(Pull pull);

        void VisitResult(Result result);

        void VisitSort(Sort sort);

        void VisitProcedure(Procedure procedure);
    }
}
