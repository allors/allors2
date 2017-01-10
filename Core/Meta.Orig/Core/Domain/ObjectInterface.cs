//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectInterface.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    [Id("12504F04-02C6-4778-98FE-04EBA12EF8B2")]
    public partial class ObjectInterface: Interface
    {
        public static ObjectInterface Instance { get; internal set; }

        private ObjectInterface() : base(MetaPopulation.Instance)
        {
        }
    }
}