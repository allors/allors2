// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactMechanismPurposes.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    public partial class ContactMechanismPurposes
    {
        private static readonly Guid RegisteredOfficeId = new Guid("8F99BE32-3817-4371-8317-8F1EF5CA2CDB");
        private static readonly Guid HeadQuartersId = new Guid("065AF4A2-44E0-4bc5-8E09-3D3A6091F841");
        private static readonly Guid SalesOfficeId = new Guid("E6E5E1AF-D490-4d9c-82CD-DD845CC70C56");
        private static readonly Guid HomeAddressId = new Guid("C371E9E3-609D-47f0-A778-DEE0A7F7B477");
        private static readonly Guid GeneralPhoneNumberId = new Guid("87467125-5CCB-49fd-9A4E-7A9A18246671");
        private static readonly Guid GeneralFaxNumberId = new Guid("33B5B453-1B38-440a-A116-3EB3C641014A");
        private static readonly Guid GeneralEmailId = new Guid("EEEAF9AB-868D-4553-ACEB-62A5409CA2B2");
        private static readonly Guid GeneralCorrespondenceId = new Guid("9E86EA3C-6EFE-4B80-9D04-5480199FE26A");
        private static readonly Guid BillingAddressId = new Guid("23FC662A-CCD0-49d7-BB48-AB3A199F070E");
        private static readonly Guid InternetAddressId = new Guid("5BA234F1-40BE-4e23-BD77-FEC9A696CB5B");
        private static readonly Guid OrderAddressId = new Guid("CB8779E1-CC02-428b-8423-F0630C970BBA");
        private static readonly Guid ShippingAddressId = new Guid("22BE08B7-8E8C-4c21-895F-DF7E66597F8B");
        private static readonly Guid BillingInquiriesPhoneId = new Guid("11196BF9-7BBB-4336-A623-C0FD3E6A444A");
        private static readonly Guid OrderInquiriesPhoneId = new Guid("952A23AF-2A5D-4456-BAB6-56F5012BC896");
        private static readonly Guid ShippingInquiriesPhoneId = new Guid("C1C7F76D-594F-4c49-B297-1E3374DECBA9");
        private static readonly Guid BillingInquiriesFaxId = new Guid("4DB3E796-0DE4-44ba-8E48-5A04121B26BD");
        private static readonly Guid OrderInquiriesFaxId = new Guid("EE74B565-F5A4-4f59-AFBA-D4070BC90F50");
        private static readonly Guid ShippingInquiriesFaxId = new Guid("5AF74DFA-65FF-4512-A0A4-0F9905C94102");
        private static readonly Guid PersonalEmailAddressId = new Guid("A3DBFB0C-3542-4a70-B2FD-EABFC34F8BF3");
        private static readonly Guid MobilePhoneNumberId = new Guid("C81E8F99-169B-4c8e-8C88-761CCCD5BBB0");
        private static readonly Guid OperationsId = new Guid("0078904B-6611-4DFA-BDC9-2A2E139ECD59");

        private UniquelyIdentifiableSticky<ContactMechanismPurpose> cache;

        public ContactMechanismPurpose RegisteredOffice => this.Cache[RegisteredOfficeId];

        public ContactMechanismPurpose HeadQuarters => this.Cache[HeadQuartersId];

        public ContactMechanismPurpose SalesOffice => this.Cache[SalesOfficeId];

        public ContactMechanismPurpose HomeAddress => this.Cache[HomeAddressId];

        public ContactMechanismPurpose GeneralCorrespondence => this.Cache[GeneralCorrespondenceId];

        public ContactMechanismPurpose GeneralFaxNumber => this.Cache[GeneralFaxNumberId];

        public ContactMechanismPurpose GeneralPhoneNumber => this.Cache[GeneralPhoneNumberId];

        public ContactMechanismPurpose GeneralEmail => this.Cache[GeneralEmailId];

        public ContactMechanismPurpose BillingAddress => this.Cache[BillingAddressId];

        public ContactMechanismPurpose InternetAddress => this.Cache[InternetAddressId];

        public ContactMechanismPurpose OrderAddress => this.Cache[OrderAddressId];

        public ContactMechanismPurpose ShippingAddress => this.Cache[ShippingAddressId];

        public ContactMechanismPurpose BillingInquiriesPhone => this.Cache[BillingInquiriesPhoneId];

        public ContactMechanismPurpose OrderInquiriesPhone => this.Cache[OrderInquiriesPhoneId];

        public ContactMechanismPurpose ShippingInquiriesPhone => this.Cache[ShippingInquiriesPhoneId];

        public ContactMechanismPurpose BillingInquiriesFax => this.Cache[BillingInquiriesFaxId];

        public ContactMechanismPurpose OrderInquiriesFax => this.Cache[OrderInquiriesFaxId];

        public ContactMechanismPurpose ShippingInquiriesFax => this.Cache[ShippingInquiriesFaxId];

        public ContactMechanismPurpose PersonalEmailAddress => this.Cache[PersonalEmailAddressId];

        public ContactMechanismPurpose MobilePhoneNumber => this.Cache[MobilePhoneNumberId];

        public ContactMechanismPurpose Operations => this.Cache[OperationsId];

        private UniquelyIdentifiableSticky<ContactMechanismPurpose> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<ContactMechanismPurpose>(this.Session));

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
        }

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Registerd Office")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Statutaire zetel").WithLocale(dutchLocale).Build())
                .WithUniqueId(RegisteredOfficeId)
                .WithIsActive(true)
                .Build();

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Head Quarters")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoofdkwartier").WithLocale(dutchLocale).Build())
                .WithUniqueId(HeadQuartersId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Sales Office")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoops bureau").WithLocale(dutchLocale).Build())
                .WithUniqueId(SalesOfficeId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Home Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Thuis adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(HomeAddressId)
                .WithIsActive(true)
                .Build();

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General correspondence address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen correspondentie adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralCorrespondenceId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General Phone Number")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen telefoonnummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralPhoneNumberId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General Fax Number")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen fax nummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralFaxNumberId)
                .WithIsActive(true)
                .Build();

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General Email Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen email adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralEmailId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Billing Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Factuuradres").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingAddressId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Internet Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Internet adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(InternetAddressId)
                .WithIsActive(true)
                .Build();
           
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Order Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ander adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderAddressId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Shipping Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verzendadres").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingAddressId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Billing Inquiries Phone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Facturatie vragen telefoon").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingInquiriesPhoneId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Order Inquiries Phone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order vragen telefoon").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderInquiriesPhoneId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Shipping Inquiries Phone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Transport vragen telefoon").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingInquiriesPhoneId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Billing Inquiries Fax")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Facturatie vragen fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingInquiriesFaxId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Order Inquiries Fax")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order vragen fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderInquiriesFaxId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Shipping Inquiries Fax")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Transport vragen fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingInquiriesFaxId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Personal Email Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Persoonlijk email adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(PersonalEmailAddressId)
                .WithIsActive(true)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Cellphone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Mobiel nummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(MobilePhoneNumberId)
                .WithIsActive(true)
                .Build();

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Operations")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Operations").WithLocale(dutchLocale).Build())
                .WithUniqueId(OperationsId)
                .WithIsActive(true)
                .Build();
        }
    }
}
