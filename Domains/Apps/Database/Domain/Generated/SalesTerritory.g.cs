// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Domain
{
	public partial class SalesTerritory : Allors.IObject , GeographicBoundaryComposite
	{
		private readonly IStrategy strategy;

		public SalesTerritory(Allors.IStrategy strategy) 
		{
			this.strategy = strategy;
		}

		public Allors.Meta.MetaSalesTerritory Meta
		{
			get
			{
				return Allors.Meta.MetaSalesTerritory.Instance;
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

		public static SalesTerritory Instantiate (Allors.ISession allorsSession, string allorsObjectId)
		{
			return (SalesTerritory) allorsSession.Instantiate(allorsObjectId);		
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


		virtual public global::Allors.Extent<GeographicBoundary> Associations
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.Associations.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.Associations.RelationType, value);
			}
		}

		virtual public void AddAssociation (GeographicBoundary value)
		{
			Strategy.AddCompositeRole(Meta.Associations.RelationType, value);
		}

		virtual public void RemoveAssociation (GeographicBoundary value)
		{
			Strategy.RemoveCompositeRole(Meta.Associations.RelationType, value);
		}

		virtual public bool ExistAssociations
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.Associations.RelationType);
			}
		}

		virtual public void RemoveAssociations()
		{
			Strategy.RemoveCompositeRoles(Meta.Associations.RelationType);
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
			var method = new SalesTerritoryOnBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnBuild OnBuild(System.Action<ObjectOnBuild> action)
		{ 
			var method = new SalesTerritoryOnBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild()
		{ 
			var method = new SalesTerritoryOnPostBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild(System.Action<ObjectOnPostBuild> action)
		{ 
			var method = new SalesTerritoryOnPostBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive()
		{ 
			var method = new SalesTerritoryOnPreDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive(System.Action<ObjectOnPreDerive> action)
		{ 
			var method = new SalesTerritoryOnPreDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive()
		{ 
			var method = new SalesTerritoryOnDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive(System.Action<ObjectOnDerive> action)
		{ 
			var method = new SalesTerritoryOnDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive()
		{ 
			var method = new SalesTerritoryOnPostDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive(System.Action<ObjectOnPostDerive> action)
		{ 
			var method = new SalesTerritoryOnPostDerive(this);
            action(method);
            method.Execute();
            return method;
		}
	}

	public partial class SalesTerritoryBuilder : Allors.ObjectBuilder<SalesTerritory> , GeographicBoundaryCompositeBuilder, global::System.IDisposable
	{		
		public SalesTerritoryBuilder(Allors.ISession session) : base(session)
	    {
	    }

		protected override void OnBuild(SalesTerritory instance)
		{

			instance.Name = this.Name;
		
		

			instance.Abbreviation = this.Abbreviation;
		
						
			

			if(this.UniqueId.HasValue)
			{
				instance.UniqueId = this.UniqueId.Value;
			}			
		
		
			if(this.Associations!=null)
			{
				instance.Associations = this.Associations.ToArray();
			}
		
			if(this.DeniedPermissions!=null)
			{
				instance.DeniedPermissions = this.DeniedPermissions.ToArray();
			}
		
			if(this.SecurityTokens!=null)
			{
				instance.SecurityTokens = this.SecurityTokens.ToArray();
			}
		
		}


				public global::System.String Name {get; set;}

				/// <exclude/>
				public SalesTerritoryBuilder WithName(global::System.String value)
		        {
				    if(this.Name!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Name = value;
		            return this;
		        }	

				public global::System.Collections.Generic.List<GeographicBoundary> Associations {get; set;}	

				/// <exclude/>
				public SalesTerritoryBuilder WithAssociation(GeographicBoundary value)
		        {
					if(this.Associations == null)
					{
						this.Associations = new global::System.Collections.Generic.List<GeographicBoundary>(); 
					}
		            this.Associations.Add(value);
		            return this;
		        }		

				
				public global::System.String Abbreviation {get; set;}

				/// <exclude/>
				public SalesTerritoryBuilder WithAbbreviation(global::System.String value)
		        {
				    if(this.Abbreviation!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Abbreviation = value;
		            return this;
		        }	

				public global::System.Collections.Generic.List<Permission> DeniedPermissions {get; set;}	

				/// <exclude/>
				public SalesTerritoryBuilder WithDeniedPermission(Permission value)
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
				public SalesTerritoryBuilder WithSecurityToken(SecurityToken value)
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
				public SalesTerritoryBuilder WithUniqueId(global::System.Guid? value)
		        {
				    if(this.UniqueId!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.UniqueId = value;
		            return this;
		        }	


	}

	public partial class SalesTerritories : global::Allors.ObjectsBase<SalesTerritory>
	{
		public SalesTerritories(Allors.ISession session) : base(session)
		{
		}

		public Allors.Meta.MetaSalesTerritory Meta
		{
			get
			{
				return Allors.Meta.MetaSalesTerritory.Instance;
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