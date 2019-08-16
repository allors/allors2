// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingletonService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

        public void Clear()
        {
            this.Id = 0;
        }
    }
}
