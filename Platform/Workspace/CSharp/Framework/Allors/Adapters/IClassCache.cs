// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClassCache.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters
{
    using Allors.Meta;

    public interface IClassCache
    {
        bool TryGet(long objectId, out IClass @class);
       
        void Set(long objectId, IClass @class);

        void Invalidate();

        void Invalidate(long[] objectsToInvalidate);
    }
}