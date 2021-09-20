// <copyright file="RequestSequence.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class RequestSequence
    {
        public bool IsEnforcedSequence => Equals(this.UniqueId, RequestSequences.EnforcedSequenceId);

        public bool IsRestartOnFiscalYear => Equals(this.UniqueId, RequestSequences.RestartOnFiscalYearId);
    }
}
