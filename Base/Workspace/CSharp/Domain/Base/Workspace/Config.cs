// ------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
// -----------------------------------------------------------------------------------------------

using Allors.Workspace.Meta;

namespace Allors.Workspace
{
    using Domain;

    public static partial class Config
    {
        public static readonly ObjectFactory ObjectFactory;

        static Config()
        {
            ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
        }
    }
}
