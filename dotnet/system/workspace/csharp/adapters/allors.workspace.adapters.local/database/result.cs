// <copyright file="LocalPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Result : IInvokeResult
    {
        private readonly List<Strategy> accessErrorStrategies;
        private readonly List<long> databaseMissingIds;
        private List<Database.Derivations.IDerivationError> derivationErrors;
        private readonly List<long> versionErrors;

        private IList<IObject> mergeErrors;

        protected Result(Session session)
        {
            this.Session = session;
            this.accessErrorStrategies = new List<Strategy>();
            this.databaseMissingIds = new List<long>();
            this.versionErrors = new List<long>();
        }

        protected Session Session { get; }

        public string ErrorMessage { get; set; }

        public IEnumerable<IObject> VersionErrors => this.versionErrors?.Select(v => this.Session.Instantiate<IObject>(v));

        public IEnumerable<IObject> AccessErrors => this.accessErrorStrategies?.Select(v => v.Object);

        public IEnumerable<IObject> MissingErrors => this.Session.Instantiate<IObject>(this.databaseMissingIds);

        public IEnumerable<IDerivationError> DerivationErrors => this.derivationErrors
            ?.Select<Database.Derivations.IDerivationError, IDerivationError>(v =>
                new DerivationError(this.Session, v)).ToArray();

        public IEnumerable<IObject> MergeErrors => this.mergeErrors ?? Array.Empty<IObject>();

        public bool HasErrors => !string.IsNullOrWhiteSpace(this.ErrorMessage) ||
                                 this.accessErrorStrategies?.Count > 0 ||
                                 this.databaseMissingIds?.Count > 0 ||
                                 this.versionErrors?.Count > 0 ||
                                 this.derivationErrors?.Count > 0 ||
                                 this.mergeErrors?.Count > 0;

        public void AddMergeError(IObject @object)
        {
            this.mergeErrors ??= new List<IObject>();
            this.mergeErrors.Add(@object);
        }

        internal void AddDerivationErrors(Database.Derivations.IDerivationError[] errors) =>
            (this.derivationErrors ??= new List<Database.Derivations.IDerivationError>()).AddRange(errors);

        internal void AddMissingId(long id) => this.databaseMissingIds.Add(id);

        internal void AddAccessError(Strategy strategy) => this.accessErrorStrategies.Add(strategy);

        internal void AddVersionError(long id) => this.versionErrors.Add(id);
    }
}
