// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupConfiguration.v.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors
{
    public partial class Setup
    {
        private void OnPrePrepare()
        {
            this.BaseOnPrePrepare();
            this.AppsOnPrePrepare();
            this.TestOnPrePrepare();
        }

        private void OnPostPrepare()
        {
            this.BaseOnPostPrepare();
            this.AppsOnPostPrepare();
            this.TestOnPostPrepare();
        }

        private void OnPreSetup()
        {
            this.BaseOnPreSetup();
            this.AppsOnPreSetup();
            this.TestOnPreSetup();
        }

        private void OnPostSetup()
        {
            this.BaseOnPostSetup();
            this.AppsOnPostSetup();
            this.TestOnPostSetup();
        }
    }
}