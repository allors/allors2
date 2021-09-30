// <copyright file="SingletonService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    public class SingletonService : ISingletonService
    {
        public SingletonService(IStateService stateService)
        {
            this.Clear();
            stateService.Register(this);
        }

        public long Id { get; set; }

        public void Clear() => this.Id = 0;
    }
}
