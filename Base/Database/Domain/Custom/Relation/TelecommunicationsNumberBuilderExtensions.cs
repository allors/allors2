// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelecommunicationsNumberBuilderExtensions.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.End2End
{
    public static partial class TelecommunicationsNumberBuilderExtensions
    {
        public static TelecommunicationsNumberBuilder WithDefaults(this TelecommunicationsNumberBuilder @this, ISession session, Config config)
        {
            if (config.End2End)
            {
                @this.WithCountryCode(config.faker.Phone.PhoneNumber("####"));
                @this.WithContactNumber(config.faker.Phone.PhoneNumber("## ## ##"));
                @this.WithDescription(config.faker.Lorem.Word());
                @this.WithContactMechanismType(config.faker.Random.ListItem(session.Extent<ContactMechanismType>()));
            }

            return @this;
        }
    }
}
