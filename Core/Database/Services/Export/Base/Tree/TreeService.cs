// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using System.Collections.Concurrent;

    using Allors.Data;
    using Allors.Meta;

    public class TreeService : ITreeService
    {
        private ConcurrentDictionary<IComposite, Tree> trees;

        public TreeService(IStateService stateService)
        {
            this.Clear();
            stateService.Register(this);
        }

        public Tree Get(IComposite type)
        {
            return this.trees.TryGetValue(type, out var tree) ? tree : null;
        }

        public void Set(IComposite type, Tree tree)
        {
            this.trees[type] = tree;
        }

        public void Clear()
        {
            this.trees = new ConcurrentDictionary<IComposite, Tree>();
        }
    }
}
