// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Domain
{
	public partial class County : Allors.IObject , GeographicBoundary, CityBound
	{
		private readonly IStrategy strategy;

		public County(Allors.IStrategy strategy) 
		{
			this.strategy = strategy;
		}

		public Allors.Meta.MetaCounty Meta
		{
			get
			{
				return Allors.Meta.MetaCounty.Instance;
			}
		}

		public long Id
		{
			get { return this.strategy.ObjectId; }
		}

		public IStrategy Strategy
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this.strategy; }
        }

		public static County Instantiate (Allors.ISession allorsSession, string allorsObjectId)
		{
			return (County) allorsSession.Instantiate(allorsObjectId);		
		}

		public override bool Equals(object obj)
        {
            var typedObj = obj as IObject;
            return typedObj != null &&
                   typedObj.Strategy.ObjectId.Equals(this.Strategy.ObjectId) &&
                   typedObj.Strategy.Session.Database.Id.Equals(this.Strategy.Session.Database.Id);
        }

		public override int GetHashCode()
        {
            return this.Strategy.ObjectId.GetHashCode();
        }



		virtual public global::System.String Name 
		{
			get
			{
				return (global::System.String) Strategy.GetUnitRole(Meta.Name.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Name.RelationType, value);
			}
		}

		virtual public bool ExistName{
			get
			{
				return Strategy.ExistUnitRole(Meta.Name.RelationType);
			}
		}

		virtual public void RemoveName()
		{
			Strategy.RemoveUnitRole(Meta.Name.RelationType);
		}


		virtual public State State
		{ 
			get
			{
				return (State) Strategy.GetCompositeRole(Meta.State.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.State.RelationType, value);
			}
		}

		virtual public bool ExistState
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.State.RelationType);
			}
		}

		virtual public void RemoveState()
		{
			Strategy.RemoveCompositeRole(Meta.State.RelationType);
		}


		virtual public global::System.String Abbreviation 
		{
			get
			{
				return (global::System.String) Strategy.GetUnitRole(Meta.Abbreviation.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Abbreviation.RelationType, value);
			}
		}

		virtual public bool ExistAbbreviation{
			get
			{
				return Strategy.ExistUnitRole(Meta.Abbreviation.RelationType);
			}
		}

		virtual public void RemoveAbbreviation()
		{
			Strategy.RemoveUnitRole(Meta.Abbreviation.RelationType);
		}


		virtual public global::System.Decimal Latitude 
		{
			get
			{
				return (global::System.Decimal) Strategy.GetUnitRole(Meta.Latitude.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Latitude.RelationType, value);
			}
		}

		virtual public bool ExistLatitude{
			get
			{
				return Strategy.ExistUnitRole(Meta.Latitude.RelationType);
			}
		}

		virtual public void RemoveLatitude()
		{
			Strategy.RemoveUnitRole(Meta.Latitude.RelationType);
		}


		virtual public global::System.Decimal Longitude 
		{
			get
			{
				return (global::System.Decimal) Strategy.GetUnitRole(Meta.Longitude.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Longitude.RelationType, value);
			}
		}

		virtual public bool ExistLongitude{
			get
			{
				return Strategy.ExistUnitRole(Meta.Longitude.RelationType);
			}
		}

		virtual public void RemoveLongitude()
		{
			Strategy.RemoveUnitRole(Meta.Longitude.RelationType);
		}


		virtual public global::Allors.Extent<Permission> DeniedPermissions
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.DeniedPermissions.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.DeniedPermissions.RelationType, value);
			}
		}

		virtual public void AddDeniedPermission (Permission value)
		{
			Strategy.AddCompositeRole(Meta.DeniedPermissions.RelationType, value);
		}

		virtual public void RemoveDeniedPermission (Permission value)
		{
			Strategy.RemoveCompositeRole(Meta.DeniedPermissions.RelationType, value);
		}

		virtual public bool ExistDeniedPermissions
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.DeniedPermissions.RelationType);
			}
		}

		virtual public void RemoveDeniedPermissions()
		{
			Strategy.RemoveCompositeRoles(Meta.DeniedPermissions.RelationType);
		}


		virtual public global::Allors.Extent<SecurityToken> SecurityTokens
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.SecurityTokens.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.SecurityTokens.RelationType, value);
			}
		}

		virtual public void AddSecurityToken (SecurityToken value)
		{
			Strategy.AddCompositeRole(Meta.SecurityTokens.RelationType, value);
		}

		virtual public void RemoveSecurityToken (SecurityToken value)
		{
			Strategy.RemoveCompositeRole(Meta.SecurityTokens.RelationType, value);
		}

		virtual public bool ExistSecurityTokens
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.SecurityTokens.RelationType);
			}
		}

		virtual public void RemoveSecurityTokens()
		{
			Strategy.RemoveCompositeRoles(Meta.SecurityTokens.RelationType);
		}


		virtual public global::System.Guid UniqueId 
		{
			get
			{
				return (global::System.Guid) Strategy.GetUnitRole(Meta.UniqueId.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.UniqueId.RelationType, value);
			}
		}

		virtual public bool ExistUniqueId{
			get
			{
				return Strategy.ExistUnitRole(Meta.UniqueId.RelationType);
			}
		}

		virtual public void RemoveUniqueId()
		{
			Strategy.RemoveUnitRole(Meta.UniqueId.RelationType);
		}


		virtual public global::Allors.Extent<City> Cities
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.Cities.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.Cities.RelationType, value);
			}
		}

		virtual public void AddCity (City value)
		{
			Strategy.AddCompositeRole(Meta.Cities.RelationType, value);
		}

		virtual public void RemoveCity (City value)
		{
			Strategy.RemoveCompositeRole(Meta.Cities.RelationType, value);
		}

		virtual public bool ExistCities
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.Cities.RelationType);
			}
		}

		virtual public void RemoveCities()
		{
			Strategy.RemoveCompositeRoles(Meta.Cities.RelationType);
		}



		virtual public global::Allors.Extent<PostalAddress> PostalAddressesWhereGeographicBoundary
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.PostalAddressesWhereGeographicBoundary.RelationType);
			}
		}

		virtual public bool ExistPostalAddressesWhereGeographicBoundary
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.PostalAddressesWhereGeographicBoundary.RelationType);
			}
		}


		virtual public global::Allors.Extent<ShippingAndHandlingComponent> ShippingAndHandlingComponentsWhereGeographicBoundary
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ShippingAndHandlingComponentsWhereGeographicBoundary.RelationType);
			}
		}

		virtual public bool ExistShippingAndHandlingComponentsWhereGeographicBoundary
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ShippingAndHandlingComponentsWhereGeographicBoundary.RelationType);
			}
		}


		virtual public global::Allors.Extent<EstimatedProductCost> EstimatedProductCostsWhereGeographicBoundary
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.EstimatedProductCostsWhereGeographicBoundary.RelationType);
			}
		}

		virtual public bool ExistEstimatedProductCostsWhereGeographicBoundary
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.EstimatedProductCostsWhereGeographicBoundary.RelationType);
			}
		}


		virtual public global::Allors.Extent<GeographicBoundaryComposite> GeographicBoundaryCompositesWhereAssociation
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.GeographicBoundaryCompositesWhereAssociation.RelationType);
			}
		}

		virtual public bool ExistGeographicBoundaryCompositesWhereAssociation
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.GeographicBoundaryCompositesWhereAssociation.RelationType);
			}
		}


		virtual public global::Allors.Extent<PriceComponent> PriceComponentsWhereGeographicBoundary
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.PriceComponentsWhereGeographicBoundary.RelationType);
			}
		}

		virtual public bool ExistPriceComponentsWhereGeographicBoundary
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.PriceComponentsWhereGeographicBoundary.RelationType);
			}
		}


		virtual public global::Allors.Extent<Notification> NotificationsWhereTarget
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.NotificationsWhereTarget.RelationType);
			}
		}

		virtual public bool ExistNotificationsWhereTarget
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.NotificationsWhereTarget.RelationType);
			}
		}



		public ObjectOnBuild OnBuild()
		{ 
			var method = new CountyOnBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnBuild OnBuild(System.Action<ObjectOnBuild> action)
		{ 
			var method = new CountyOnBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild()
		{ 
			var method = new CountyOnPostBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild(System.Action<ObjectOnPostBuild> action)
		{ 
			var method = new CountyOnPostBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive()
		{ 
			var method = new CountyOnPreDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive(System.Action<ObjectOnPreDerive> action)
		{ 
			var method = new CountyOnPreDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive()
		{ 
			var method = new CountyOnDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive(System.Action<ObjectOnDerive> action)
		{ 
			var method = new CountyOnDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive()
		{ 
			var method = new CountyOnPostDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive(System.Action<ObjectOnPostDerive> action)
		{ 
			var method = new CountyOnPostDerive(this);
            action(method);
            method.Execute();
            return method;
		}
	}

	public partial class CountyBuilder : Allors.ObjectBuilder<County> , GeographicBoundaryBuilder, CityBoundBuilder, global::System.IDisposable
	{		
		public CountyBuilder(Allors.ISession session) : base(session)
	    {
	    }

		protected override void OnBuild(County instance)
		{

			instance.Name = this.Name;
		
		

			instance.Abbreviation = this.Abbreviation;
		
						
			

			if(this.UniqueId.HasValue)
			{
				instance.UniqueId = this.UniqueId.Value;
			}			
		
		

			instance.State = this.State;

		
			if(this.DeniedPermissions!=null)
			{
				instance.DeniedPermissions = this.DeniedPermissions.ToArray();
			}
		
			if(this.SecurityTokens!=null)
			{
				instance.SecurityTokens = this.SecurityTokens.ToArray();
			}
		
			if(this.Cities!=null)
			{
				instance.Cities = this.Cities.ToArray();
			}
		
		}


				public global::System.String Name {get; set;}

				/// <exclude/>
				public CountyBuilder WithName(global::System.String value)
		        {
				    if(this.Name!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Name = value;
		            return this;
		        }	

				public State State {get; set;}

				/// <exclude/>
				public CountyBuilder WithState(State value)
		        {
		            if(this.State!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.State = value;
		            return this;
		        }		

				
				public global::System.String Abbreviation {get; set;}

				/// <exclude/>
				public CountyBuilder WithAbbreviation(global::System.String value)
		        {
				    if(this.Abbreviation!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Abbreviation = value;
		            return this;
		        }	

				public global::System.Collections.Generic.List<Permission> DeniedPermissions {get; set;}	

				/// <exclude/>
				public CountyBuilder WithDeniedPermission(Permission value)
		        {
					if(this.DeniedPermissions == null)
					{
						this.DeniedPermissions = new global::System.Collections.Generic.List<Permission>(); 
					}
		            this.DeniedPermissions.Add(value);
		            return this;
		        }		

				
				public global::System.Collections.Generic.List<SecurityToken> SecurityTokens {get; set;}	

				/// <exclude/>
				public CountyBuilder WithSecurityToken(SecurityToken value)
		        {
					if(this.SecurityTokens == null)
					{
						this.SecurityTokens = new global::System.Collections.Generic.List<SecurityToken>(); 
					}
		            this.SecurityTokens.Add(value);
		            return this;
		        }		

				
				public global::System.Guid? UniqueId {get; set;}

				/// <exclude/>
				public CountyBuilder WithUniqueId(global::System.Guid? value)
		        {
				    if(this.UniqueId!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.UniqueId = value;
		            return this;
		        }	

				public global::System.Collections.Generic.List<City> Cities {get; set;}	

				/// <exclude/>
				public CountyBuilder WithCity(City value)
		        {
					if(this.Cities == null)
					{
						this.Cities = new global::System.Collections.Generic.List<City>(); 
					}
		            this.Cities.Add(value);
		            return this;
		        }		

				

	}

	public partial class Counties : global::Allors.ObjectsBase<County>
	{
		public Counties(Allors.ISession session) : base(session)
		{
		}

		public Allors.Meta.MetaCounty Meta
		{
			get
			{
				return Allors.Meta.MetaCounty.Instance;
			}
		}

		public override Allors.Meta.Composite ObjectType
		{
			get
			{
				return Meta.ObjectType;
			}
		}
	}

}