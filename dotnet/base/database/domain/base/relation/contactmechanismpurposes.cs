// <copyright file="ContactMechanismPurposes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

        private UniquelyIdentifiableSticky<ContactMechanismPurpose> Cache => this.cache ??= new UniquelyIdentifiableSticky<ContactMechanismPurpose>(this.Session);

        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.Locale.ObjectType);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(RegisteredOfficeId, v =>
            {
                v.Name = "Registerd Office";
                localisedName.Set(v, dutchLocale, "Statutaire zetel");
                v.IsActive = true;
            });

            merge(HeadQuartersId, v =>
            {
                v.Name = "Head Quarters";
                localisedName.Set(v, dutchLocale, "Hoofdkwartier");
                v.IsActive = true;
            });

            merge(SalesOfficeId, v =>
            {
                v.Name = "Sales Office";
                localisedName.Set(v, dutchLocale, "Verkoops bureau");
                v.IsActive = true;
            });

            merge(HomeAddressId, v =>
            {
                v.Name = "Home Address";
                localisedName.Set(v, dutchLocale, "Thuis adres");
                v.IsActive = true;
            });

            merge(GeneralCorrespondenceId, v =>
            {
                v.Name = "General correspondence address";
                localisedName.Set(v, dutchLocale, "Algemeen correspondentie adres");
                v.IsActive = true;
            });

            merge(GeneralPhoneNumberId, v =>
            {
                v.Name = "General Phone Number";
                localisedName.Set(v, dutchLocale, "Algemeen telefoonnummer");
                v.IsActive = true;
            });

            merge(GeneralFaxNumberId, v =>
            {
                v.Name = "General Fax Number";
                localisedName.Set(v, dutchLocale, "Algemeen fax nummer");
                v.IsActive = true;
            });

            merge(GeneralEmailId, v =>
            {
                v.Name = "General Email Address";
                localisedName.Set(v, dutchLocale, "Algemeen email adres");
                v.IsActive = true;
            });

            merge(BillingAddressId, v =>
            {
                v.Name = "Billing Address";
                localisedName.Set(v, dutchLocale, "Factuuradres");
                v.IsActive = true;
            });

            merge(InternetAddressId, v =>
            {
                v.Name = "Internet Address";
                localisedName.Set(v, dutchLocale, "Internet adres");
                v.IsActive = true;
            });

            merge(OrderAddressId, v =>
            {
                v.Name = "Order Address";
                localisedName.Set(v, dutchLocale, "Order adres");
                v.IsActive = true;
            });

            merge(ShippingAddressId, v =>
            {
                v.Name = "Shipping Address";
                localisedName.Set(v, dutchLocale, "Verzendadres");
                v.IsActive = true;
            });

            merge(BillingInquiriesPhoneId, v =>
            {
                v.Name = "Billing Inquiries Phone";
                localisedName.Set(v, dutchLocale, "Facturatie vragen telefoon");
                v.IsActive = true;
            });

            merge(OrderInquiriesPhoneId, v =>
            {
                v.Name = "Order Inquiries Phone";
                localisedName.Set(v, dutchLocale, "Order vragen telefoon");
                v.IsActive = true;
            });

            merge(ShippingInquiriesPhoneId, v =>
            {
                v.Name = "Shipping Inquiries Phone";
                localisedName.Set(v, dutchLocale, "Transport vragen telefoon");
                v.IsActive = true;
            });

            merge(BillingInquiriesFaxId, v =>
            {
                v.Name = "Billing Inquiries Fax";
                localisedName.Set(v, dutchLocale, "Facturatie vragen fax");
                v.IsActive = true;
            });

            merge(OrderInquiriesFaxId, v =>
            {
                v.Name = "Order Inquiries Fax";
                localisedName.Set(v, dutchLocale, "Order vragen fax");
                v.IsActive = true;
            });

            merge(ShippingInquiriesFaxId, v =>
            {
                v.Name = "Shipping Inquiries Fax";
                localisedName.Set(v, dutchLocale, "Transport vragen fax");
                v.IsActive = true;
            });

            merge(PersonalEmailAddressId, v =>
            {
                v.Name = "Personal Email Address";
                localisedName.Set(v, dutchLocale, "Persoonlijk email adres");
                v.IsActive = true;
            });

            merge(MobilePhoneNumberId, v =>
            {
                v.Name = "Cellphone";
                localisedName.Set(v, dutchLocale, "Mobiel nummer");
                v.IsActive = true;
            });

            merge(OperationsId, v =>
            {
                v.Name = "Operations";
                localisedName.Set(v, dutchLocale, "Operations");
                v.IsActive = true;
            });
        }
    }
}
