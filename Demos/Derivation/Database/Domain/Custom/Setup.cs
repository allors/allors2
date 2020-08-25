// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setup.cs" company="Allors bvba">
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
// --------------------------------------------------------------------------------------------------------------------

using Allors.Domain;

namespace Allors
{
    public partial class Setup
    {
        private void CustomOnPrePrepare()
        {
        }

        private void CustomOnPostPrepare()
        {
        }

        private void CustomOnPreSetup()
        {
        }

        private void CustomOnPostSetup()
        {
            var speler1 = new PersonBuilder(this.session)
                .WithUserName("speler1")
                .WithFirstName("Speler")
                .WithLastName("Een")
                .Build();

            var speler2 = new PersonBuilder(this.session)
                .WithUserName("speler2")
                .WithFirstName("Speler")
                .WithLastName("Twee")
                .Build();

            var speler3 = new PersonBuilder(this.session)
                .WithUserName("speler3")
                .WithFirstName("Speler")
                .WithLastName("Drie")
                .Build();

            var speler4 = new PersonBuilder(this.session)
                .WithUserName("speler4")
                .WithFirstName("Speler")
                .WithLastName("Vier")
                .Build();

            var speler5 = new PersonBuilder(this.session)
                .WithUserName("speler5")
                .WithFirstName("Speler")
                .WithLastName("Vijf")
                .Build();
        }
    }
}
