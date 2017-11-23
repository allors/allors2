namespace Allors.Workspace.Meta
{
	using Allors.Meta;

	public static class M
	{
		// Interfaces
        public static readonly MetaVersion Version;
        public static readonly MetaDeletable Deletable;
        public static readonly MetaEnumeration Enumeration;
        public static readonly MetaLocalised Localised;
        public static readonly MetaObjectState ObjectState;
        public static readonly MetaUniquelyIdentifiable UniquelyIdentifiable;
        public static readonly MetaUser User;
        public static readonly MetaTask Task;
        public static readonly MetaWorkItem WorkItem;
        public static readonly MetaI1 I1;

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
        public static readonly MetaNotification Notification;
        public static readonly MetaNotificationList NotificationList;
        public static readonly MetaTaskAssignment TaskAssignment;
        public static readonly MetaTaskList TaskList;
        public static readonly MetaC1 C1;
        public static readonly MetaDependent Dependent;
        public static readonly MetaGender Gender;
        public static readonly MetaOrder Order;
        public static readonly MetaOrderLine OrderLine;
        public static readonly MetaOrderLineVersion OrderLineVersion;
        public static readonly MetaPaymentState PaymentState;
        public static readonly MetaShipmentState ShipmentState;
        public static readonly MetaOrderState OrderState;
        public static readonly MetaOrderVersion OrderVersion;
        public static readonly MetaOrganisation Organisation;
        public static readonly MetaUnitSample UnitSample;


		static M()
		{
		    // Interfaces
            Version = MetaVersion.Instance;
            Deletable = MetaDeletable.Instance;
            Enumeration = MetaEnumeration.Instance;
            Localised = MetaLocalised.Instance;
            ObjectState = MetaObjectState.Instance;
            UniquelyIdentifiable = MetaUniquelyIdentifiable.Instance;
            User = MetaUser.Instance;
            Task = MetaTask.Instance;
            WorkItem = MetaWorkItem.Instance;
            I1 = MetaI1.Instance;

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
            Notification = MetaNotification.Instance;
            NotificationList = MetaNotificationList.Instance;
            TaskAssignment = MetaTaskAssignment.Instance;
            TaskList = MetaTaskList.Instance;
            C1 = MetaC1.Instance;
            Dependent = MetaDependent.Instance;
            Gender = MetaGender.Instance;
            Order = MetaOrder.Instance;
            OrderLine = MetaOrderLine.Instance;
            OrderLineVersion = MetaOrderLineVersion.Instance;
            PaymentState = MetaPaymentState.Instance;
            ShipmentState = MetaShipmentState.Instance;
            OrderState = MetaOrderState.Instance;
            OrderVersion = MetaOrderVersion.Instance;
            Organisation = MetaOrganisation.Instance;
            UnitSample = MetaUnitSample.Instance;
		}
	}

	public partial class MetaInterface 
	{
		public void Extend()
		{
			this.CoreExtend();
			this.CustomExtend();
			this.BaseExtend();
			this.CoreExtend();
		}

       internal virtual void CustomExtend() {}
       internal virtual void BaseExtend() {}
       internal virtual void CoreExtend() {}
	}

	public partial class MetaClass 
	{
		public void Extend()
		{
			this.CoreExtend();
			this.CustomExtend();
			this.BaseExtend();
			this.CoreExtend();
		}

       internal virtual void CustomExtend() {}
       internal virtual void BaseExtend() {}
       internal virtual void CoreExtend() {}
	}
}