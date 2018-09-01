// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
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