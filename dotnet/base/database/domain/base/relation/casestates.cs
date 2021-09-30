// <copyright file="CaseStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class CaseStates
    {
        private static readonly Guid ReadId = new Guid("595CD7D4-8CB5-463b-8661-8859B1A0484D");
        private static readonly Guid ClosedId = new Guid("F13E2DE5-32C0-4d6b-9949-F6D60B087A8A");
        private static readonly Guid InProgressId = new Guid("5C5B605F-ABF3-4956-A289-CA4AD3B3B4BE");
        private static readonly Guid CompletedId = new Guid("8203E84A-3299-448a-982E-4A79534CAB85");
        private static readonly Guid OpenedId = new Guid("4FF18EE3-C827-47a8-A5DE-EFA29CE9BB68");

        private UniquelyIdentifiableSticky<CaseState> cache;

        public CaseState Opened => this.Cache[OpenedId];

        public CaseState Closed => this.Cache[ClosedId];

        public CaseState Read => this.Cache[ReadId];

        public CaseState InProgress => this.Cache[InProgressId];

        public CaseState Completed => this.Cache[CompletedId];

        private UniquelyIdentifiableSticky<CaseState> Cache => this.cache ??= new UniquelyIdentifiableSticky<CaseState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(ClosedId, v => v.Name = "Closed");
            merge(CompletedId, v => v.Name = "Completed");
            merge(InProgressId, v => v.Name = "In Progress");
            merge(OpenedId, v => v.Name = "Open");
            merge(ReadId, v => v.Name = "Read");
        }
    }
}
