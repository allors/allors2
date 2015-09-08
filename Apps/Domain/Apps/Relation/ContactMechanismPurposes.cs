// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactMechanismPurposes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Domain
{
    using System;

    public partial class ContactMechanismPurposes
    {
        public static readonly Guid HeadQuartersId = new Guid("065AF4A2-44E0-4bc5-8E09-3D3A6091F841");
        public static readonly Guid SalesOfficeId = new Guid("E6E5E1AF-D490-4d9c-82CD-DD845CC70C56");
        public static readonly Guid HomeAddressId = new Guid("C371E9E3-609D-47f0-A778-DEE0A7F7B477");
        public static readonly Guid GeneralPhoneNumberId = new Guid("87467125-5CCB-49fd-9A4E-7A9A18246671");
        public static readonly Guid GeneralFaxNumberId = new Guid("33B5B453-1B38-440a-A116-3EB3C641014A");
        public static readonly Guid GeneralEmailId = new Guid("EEEAF9AB-868D-4553-ACEB-62A5409CA2B2");
        public static readonly Guid GeneralCorrespondenceId = new Guid("9E86EA3C-6EFE-4B80-9D04-5480199FE26A");
        public static readonly Guid BillingAddressId = new Guid("23FC662A-CCD0-49d7-BB48-AB3A199F070E");
        public static readonly Guid InternetAddressId = new Guid("5BA234F1-40BE-4e23-BD77-FEC9A696CB5B");
        public static readonly Guid OrderAddressId = new Guid("CB8779E1-CC02-428b-8423-F0630C970BBA");
        public static readonly Guid ShippingAddressId = new Guid("22BE08B7-8E8C-4c21-895F-DF7E66597F8B");
        public static readonly Guid BillingInquiriesPhoneId = new Guid("11196BF9-7BBB-4336-A623-C0FD3E6A444A");
        public static readonly Guid OrderInquiriesPhoneId = new Guid("952A23AF-2A5D-4456-BAB6-56F5012BC896");
        public static readonly Guid ShippingInquiriesPhoneId = new Guid("C1C7F76D-594F-4c49-B297-1E3374DECBA9");
        public static readonly Guid BillingInquiriesFaxId = new Guid("4DB3E796-0DE4-44ba-8E48-5A04121B26BD");
        public static readonly Guid OrderInquiriesFaxId = new Guid("EE74B565-F5A4-4f59-AFBA-D4070BC90F50");
        public static readonly Guid ShippingInquiriesFaxId = new Guid("5AF74DFA-65FF-4512-A0A4-0F9905C94102");
        public static readonly Guid PersonalEmailAddressId = new Guid("A3DBFB0C-3542-4a70-B2FD-EABFC34F8BF3");
        public static readonly Guid CellPhoneNumberId = new Guid("C81E8F99-169B-4c8e-8C88-761CCCD5BBB0");

        private UniquelyIdentifiableCache<ContactMechanismPurpose> cache;

        public ContactMechanismPurpose HeadQuarters
        {
            get { return this.Cache.Get(HeadQuartersId); }
        }

        public ContactMechanismPurpose SalesOffice
        {
            get { return this.Cache.Get(SalesOfficeId); }
        }

        public ContactMechanismPurpose HomeAddress
        {
            get { return this.Cache.Get(HomeAddressId); }
        }

        public ContactMechanismPurpose GeneralCorrespondence
        {
            get { return this.Cache.Get(GeneralCorrespondenceId); }
        }

        public ContactMechanismPurpose GeneralFaxNumber
        {
            get { return this.Cache.Get(GeneralFaxNumberId); }
        }

        public ContactMechanismPurpose GeneralPhoneNumber
        {
            get { return this.Cache.Get(GeneralPhoneNumberId); }
        }

        public ContactMechanismPurpose GeneralEmail
        {
            get { return this.Cache.Get(GeneralEmailId); }
        }

        public ContactMechanismPurpose BillingAddress
        {
            get { return this.Cache.Get(BillingAddressId); }
        }

        public ContactMechanismPurpose InternetAddress
        {
            get { return this.Cache.Get(InternetAddressId); }
        }

        public ContactMechanismPurpose OrderAddress
        {
            get { return this.Cache.Get(OrderAddressId); }
        }

        public ContactMechanismPurpose ShippingAddress
        {
            get { return this.Cache.Get(ShippingAddressId); }
        }

        public ContactMechanismPurpose BillingInquiriesPhone
        {
            get { return this.Cache.Get(BillingInquiriesPhoneId); }
        }

        public ContactMechanismPurpose OrderInquiriesPhone
        {
            get { return this.Cache.Get(OrderInquiriesPhoneId); }
        }

        public ContactMechanismPurpose ShippingInquiriesPhone
        {
            get { return this.Cache.Get(ShippingInquiriesPhoneId); }
        }

        public ContactMechanismPurpose BillingInquiriesFax
        {
            get { return this.Cache.Get(BillingInquiriesFaxId); }
        }

        public ContactMechanismPurpose OrderInquiriesFax
        {
            get { return this.Cache.Get(OrderInquiriesFaxId); }
        }

        public ContactMechanismPurpose ShippingInquiriesFax
        {
            get { return this.Cache.Get(ShippingInquiriesFaxId); }
        }

        public ContactMechanismPurpose PersonalEmailAddress
        {
            get { return this.Cache.Get(PersonalEmailAddressId); }
        }

        public ContactMechanismPurpose CellPhoneNumber
        {
            get { return this.Cache.Get(CellPhoneNumberId); }
        }

        private UniquelyIdentifiableCache<ContactMechanismPurpose> Cache
        {
            get 
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<ContactMechanismPurpose>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Head Quarters")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Head Quarters").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoofdkwartier").WithLocale(dutchLocale).Build())
                .WithUniqueId(HeadQuartersId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Sales Office")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Sales Office").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoops bureau").WithLocale(dutchLocale).Build())
                .WithUniqueId(SalesOfficeId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Home Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Home Address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Thuis adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(HomeAddressId)
                .Build();

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General Correspondence Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("General correspondence address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen correspondentie adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralCorrespondenceId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General Phone Number")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("General Phone Number").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen telefoonnummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralPhoneNumberId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General Fax Number")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("General Fax Number").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen fax nummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralFaxNumberId)
                .Build();

            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("General Email Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("General Email Address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Algemeen email adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralEmailId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Billing Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing Address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Factuuradres").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingAddressId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Internet Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Internet Address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Internet adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(InternetAddressId)
                .Build();
           
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Order Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order Address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ander adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderAddressId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Shipping Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shipping Address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verzendadres").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingAddressId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Billing Inquiries Phone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing Inquiries Phone").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Facturatie vragen telefoon").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingInquiriesPhoneId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Order Inquiries Phone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order Inquiries Phone").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order vragen telefoon").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderInquiriesPhoneId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Shipping Inquiries Phone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shipping Inquiries Phone").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Transport vragen telefoon").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingInquiriesPhoneId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Billing Inquiries Fax")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing Inquiries Fax").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Facturatie vragen fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingInquiriesFaxId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Order Inquiries Fax")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order Inquiries Fax").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order vragen fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderInquiriesFaxId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Shipping Inquiries Fax")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shipping Inquiries Fax").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Transport vragen fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingInquiriesFaxId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Personal Email Address")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Personal Email Address").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Persoonlijk email adres").WithLocale(dutchLocale).Build())
                .WithUniqueId(PersonalEmailAddressId)
                .Build();
            
            new ContactMechanismPurposeBuilder(this.Session)
                .WithName("Cellphone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cellphone").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Mobiel nummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(CellPhoneNumberId)
                .Build();
        }
        
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
