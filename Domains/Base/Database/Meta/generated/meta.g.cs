namespace Allors.Meta
{
	public static class M
	{
		// Interfaces
        public static readonly MetaObject Object;
        public static readonly MetaCachable Cachable;
        public static readonly MetaVersion Version;
        public static readonly MetaVersioned Versioned;
        public static readonly MetaAccessControlledObject AccessControlledObject;
        public static readonly MetaDeletable Deletable;
        public static readonly MetaEnumeration Enumeration;
        public static readonly MetaLocalised Localised;
        public static readonly MetaObjectState ObjectState;
        public static readonly MetaSecurityTokenOwner SecurityTokenOwner;
        public static readonly MetaTransitionalVersion TransitionalVersion;
        public static readonly MetaTransitional Transitional;
        public static readonly MetaUniquelyIdentifiable UniquelyIdentifiable;
        public static readonly MetaUser User;
        public static readonly MetaTask Task;
        public static readonly MetaWorkItem WorkItem;
        public static readonly MetaAddress Address;
        public static readonly MetaI1 I1;
        public static readonly MetaI12 I12;
        public static readonly MetaI2 I2;
        public static readonly MetaS1 S1;
        public static readonly MetaShared Shared;
        public static readonly MetaSyncDepth1 SyncDepth1;
        public static readonly MetaSyncDepth2 SyncDepth2;
        public static readonly MetaSyncRoot SyncRoot;
        public static readonly MetaValidationI12 ValidationI12;

		// Classes
        public static readonly MetaLocalisedText LocalisedText;
        public static readonly MetaAccessControl AccessControl;
        public static readonly MetaCounter Counter;
        public static readonly MetaCountry Country;
        public static readonly MetaCurrency Currency;
        public static readonly MetaLanguage Language;
        public static readonly MetaLocale Locale;
        public static readonly MetaLogin Login;
        public static readonly MetaMedia Media;
        public static readonly MetaMediaContent MediaContent;
        public static readonly MetaAutomatedAgent AutomatedAgent;
        public static readonly MetaPermission Permission;
        public static readonly MetaPerson Person;
        public static readonly MetaRole Role;
        public static readonly MetaSecurityToken SecurityToken;
        public static readonly MetaSingleton Singleton;
        public static readonly MetaUserGroup UserGroup;
        public static readonly MetaEmailMessage EmailMessage;
        public static readonly MetaNotification Notification;
        public static readonly MetaNotificationList NotificationList;
        public static readonly MetaTaskAssignment TaskAssignment;
        public static readonly MetaTaskList TaskList;
        public static readonly MetaBuild Build;
        public static readonly MetaBadUI BadUI;
        public static readonly MetaC1 C1;
        public static readonly MetaC2 C2;
        public static readonly MetaClassWithoutRoles ClassWithoutRoles;
        public static readonly MetaDependee Dependee;
        public static readonly MetaDependent Dependent;
        public static readonly MetaExtender Extender;
        public static readonly MetaFirst First;
        public static readonly MetaFour Four;
        public static readonly MetaFrom From;
        public static readonly MetaGender Gender;
        public static readonly MetaHomeAddress HomeAddress;
        public static readonly MetaMailboxAddress MailboxAddress;
        public static readonly MetaOne One;
        public static readonly MetaOrder Order;
        public static readonly MetaOrderLine OrderLine;
        public static readonly MetaOrderLineVersion OrderLineVersion;
        public static readonly MetaPaymentState PaymentState;
        public static readonly MetaShipmentState ShipmentState;
        public static readonly MetaOrderState OrderState;
        public static readonly MetaOrderVersion OrderVersion;
        public static readonly MetaOrganisation Organisation;
        public static readonly MetaPlace Place;
        public static readonly MetaSecond Second;
        public static readonly MetaSimpleJob SimpleJob;
        public static readonly MetaStatefulCompany StatefulCompany;
        public static readonly MetaSubdependee Subdependee;
        public static readonly MetaThird Third;
        public static readonly MetaThree Three;
        public static readonly MetaTo To;
        public static readonly MetaTwo Two;
        public static readonly MetaUnitSample UnitSample;
        public static readonly MetaValidationC1 ValidationC1;
        public static readonly MetaValidationC2 ValidationC2;


		static M()
		{
		    // Interfaces
            Object = MetaObject.Instance;
            Cachable = MetaCachable.Instance;
            Version = MetaVersion.Instance;
            Versioned = MetaVersioned.Instance;
            AccessControlledObject = MetaAccessControlledObject.Instance;
            Deletable = MetaDeletable.Instance;
            Enumeration = MetaEnumeration.Instance;
            Localised = MetaLocalised.Instance;
            ObjectState = MetaObjectState.Instance;
            SecurityTokenOwner = MetaSecurityTokenOwner.Instance;
            TransitionalVersion = MetaTransitionalVersion.Instance;
            Transitional = MetaTransitional.Instance;
            UniquelyIdentifiable = MetaUniquelyIdentifiable.Instance;
            User = MetaUser.Instance;
            Task = MetaTask.Instance;
            WorkItem = MetaWorkItem.Instance;
            Address = MetaAddress.Instance;
            I1 = MetaI1.Instance;
            I12 = MetaI12.Instance;
            I2 = MetaI2.Instance;
            S1 = MetaS1.Instance;
            Shared = MetaShared.Instance;
            SyncDepth1 = MetaSyncDepth1.Instance;
            SyncDepth2 = MetaSyncDepth2.Instance;
            SyncRoot = MetaSyncRoot.Instance;
            ValidationI12 = MetaValidationI12.Instance;

		    // Classes
            LocalisedText = MetaLocalisedText.Instance;
            AccessControl = MetaAccessControl.Instance;
            Counter = MetaCounter.Instance;
            Country = MetaCountry.Instance;
            Currency = MetaCurrency.Instance;
            Language = MetaLanguage.Instance;
            Locale = MetaLocale.Instance;
            Login = MetaLogin.Instance;
            Media = MetaMedia.Instance;
            MediaContent = MetaMediaContent.Instance;
            AutomatedAgent = MetaAutomatedAgent.Instance;
            Permission = MetaPermission.Instance;
            Person = MetaPerson.Instance;
            Role = MetaRole.Instance;
            SecurityToken = MetaSecurityToken.Instance;
            Singleton = MetaSingleton.Instance;
            UserGroup = MetaUserGroup.Instance;
            EmailMessage = MetaEmailMessage.Instance;
            Notification = MetaNotification.Instance;
            NotificationList = MetaNotificationList.Instance;
            TaskAssignment = MetaTaskAssignment.Instance;
            TaskList = MetaTaskList.Instance;
            Build = MetaBuild.Instance;
            BadUI = MetaBadUI.Instance;
            C1 = MetaC1.Instance;
            C2 = MetaC2.Instance;
            ClassWithoutRoles = MetaClassWithoutRoles.Instance;
            Dependee = MetaDependee.Instance;
            Dependent = MetaDependent.Instance;
            Extender = MetaExtender.Instance;
            First = MetaFirst.Instance;
            Four = MetaFour.Instance;
            From = MetaFrom.Instance;
            Gender = MetaGender.Instance;
            HomeAddress = MetaHomeAddress.Instance;
            MailboxAddress = MetaMailboxAddress.Instance;
            One = MetaOne.Instance;
            Order = MetaOrder.Instance;
            OrderLine = MetaOrderLine.Instance;
            OrderLineVersion = MetaOrderLineVersion.Instance;
            PaymentState = MetaPaymentState.Instance;
            ShipmentState = MetaShipmentState.Instance;
            OrderState = MetaOrderState.Instance;
            OrderVersion = MetaOrderVersion.Instance;
            Organisation = MetaOrganisation.Instance;
            Place = MetaPlace.Instance;
            Second = MetaSecond.Instance;
            SimpleJob = MetaSimpleJob.Instance;
            StatefulCompany = MetaStatefulCompany.Instance;
            Subdependee = MetaSubdependee.Instance;
            Third = MetaThird.Instance;
            Three = MetaThree.Instance;
            To = MetaTo.Instance;
            Two = MetaTwo.Instance;
            UnitSample = MetaUnitSample.Instance;
            ValidationC1 = MetaValidationC1.Instance;
            ValidationC2 = MetaValidationC2.Instance;
		}
	}

	public partial class MetaInterface 
	{
		public void Extend()
		{
			this.CoreExtend();
			this.BaseExtend();
			this.CustomExtend();
		}

       internal virtual void CoreExtend() {}
       internal virtual void BaseExtend() {}
       internal virtual void CustomExtend() {}
	}

	public partial class MetaClass 
	{
		public void Extend()
		{
			this.CoreExtend();
			this.BaseExtend();
			this.CustomExtend();
		}

       internal virtual void CoreExtend() {}
       internal virtual void BaseExtend() {}
       internal virtual void CustomExtend() {}
	}
}