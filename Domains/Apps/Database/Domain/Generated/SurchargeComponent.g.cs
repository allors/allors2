// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Domain
{
	public partial class SurchargeComponent : Allors.IObject , PriceComponent
	{
		private readonly IStrategy strategy;

		public SurchargeComponent(Allors.IStrategy strategy) 
		{
			this.strategy = strategy;
		}

		public Allors.Meta.MetaSurchargeComponent Meta
		{
			get
			{
				return Allors.Meta.MetaSurchargeComponent.Instance;
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

		public static SurchargeComponent Instantiate (Allors.ISession allorsSession, string allorsObjectId)
		{
			return (SurchargeComponent) allorsSession.Instantiate(allorsObjectId);		
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



		virtual public global::System.Decimal? Percentage 
		{
			get
			{
				return (global::System.Decimal?) Strategy.GetUnitRole(Meta.Percentage.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Percentage.RelationType, value);
			}
		}

		virtual public bool ExistPercentage{
			get
			{
				return Strategy.ExistUnitRole(Meta.Percentage.RelationType);
			}
		}

		virtual public void RemovePercentage()
		{
			Strategy.RemoveUnitRole(Meta.Percentage.RelationType);
		}


		virtual public GeographicBoundary GeographicBoundary
		{ 
			get
			{
				return (GeographicBoundary) Strategy.GetCompositeRole(Meta.GeographicBoundary.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.GeographicBoundary.RelationType, value);
			}
		}

		virtual public bool ExistGeographicBoundary
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.GeographicBoundary.RelationType);
			}
		}

		virtual public void RemoveGeographicBoundary()
		{
			Strategy.RemoveCompositeRole(Meta.GeographicBoundary.RelationType);
		}


		virtual public global::System.Decimal? Rate 
		{
			get
			{
				return (global::System.Decimal?) Strategy.GetUnitRole(Meta.Rate.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Rate.RelationType, value);
			}
		}

		virtual public bool ExistRate{
			get
			{
				return Strategy.ExistUnitRole(Meta.Rate.RelationType);
			}
		}

		virtual public void RemoveRate()
		{
			Strategy.RemoveUnitRole(Meta.Rate.RelationType);
		}


		virtual public RevenueValueBreak RevenueValueBreak
		{ 
			get
			{
				return (RevenueValueBreak) Strategy.GetCompositeRole(Meta.RevenueValueBreak.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.RevenueValueBreak.RelationType, value);
			}
		}

		virtual public bool ExistRevenueValueBreak
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.RevenueValueBreak.RelationType);
			}
		}

		virtual public void RemoveRevenueValueBreak()
		{
			Strategy.RemoveCompositeRole(Meta.RevenueValueBreak.RelationType);
		}


		virtual public PartyClassification PartyClassification
		{ 
			get
			{
				return (PartyClassification) Strategy.GetCompositeRole(Meta.PartyClassification.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.PartyClassification.RelationType, value);
			}
		}

		virtual public bool ExistPartyClassification
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.PartyClassification.RelationType);
			}
		}

		virtual public void RemovePartyClassification()
		{
			Strategy.RemoveCompositeRole(Meta.PartyClassification.RelationType);
		}


		virtual public OrderQuantityBreak OrderQuantityBreak
		{ 
			get
			{
				return (OrderQuantityBreak) Strategy.GetCompositeRole(Meta.OrderQuantityBreak.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.OrderQuantityBreak.RelationType, value);
			}
		}

		virtual public bool ExistOrderQuantityBreak
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.OrderQuantityBreak.RelationType);
			}
		}

		virtual public void RemoveOrderQuantityBreak()
		{
			Strategy.RemoveCompositeRole(Meta.OrderQuantityBreak.RelationType);
		}


		virtual public PackageQuantityBreak PackageQuantityBreak
		{ 
			get
			{
				return (PackageQuantityBreak) Strategy.GetCompositeRole(Meta.PackageQuantityBreak.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.PackageQuantityBreak.RelationType, value);
			}
		}

		virtual public bool ExistPackageQuantityBreak
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.PackageQuantityBreak.RelationType);
			}
		}

		virtual public void RemovePackageQuantityBreak()
		{
			Strategy.RemoveCompositeRole(Meta.PackageQuantityBreak.RelationType);
		}


		virtual public Product Product
		{ 
			get
			{
				return (Product) Strategy.GetCompositeRole(Meta.Product.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.Product.RelationType, value);
			}
		}

		virtual public bool ExistProduct
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.Product.RelationType);
			}
		}

		virtual public void RemoveProduct()
		{
			Strategy.RemoveCompositeRole(Meta.Product.RelationType);
		}


		virtual public RevenueQuantityBreak RevenueQuantityBreak
		{ 
			get
			{
				return (RevenueQuantityBreak) Strategy.GetCompositeRole(Meta.RevenueQuantityBreak.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.RevenueQuantityBreak.RelationType, value);
			}
		}

		virtual public bool ExistRevenueQuantityBreak
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.RevenueQuantityBreak.RelationType);
			}
		}

		virtual public void RemoveRevenueQuantityBreak()
		{
			Strategy.RemoveCompositeRole(Meta.RevenueQuantityBreak.RelationType);
		}


		virtual public ProductFeature ProductFeature
		{ 
			get
			{
				return (ProductFeature) Strategy.GetCompositeRole(Meta.ProductFeature.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.ProductFeature.RelationType, value);
			}
		}

		virtual public bool ExistProductFeature
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.ProductFeature.RelationType);
			}
		}

		virtual public void RemoveProductFeature()
		{
			Strategy.RemoveCompositeRole(Meta.ProductFeature.RelationType);
		}


		virtual public AgreementPricingProgram AgreementPricingProgram
		{ 
			get
			{
				return (AgreementPricingProgram) Strategy.GetCompositeRole(Meta.AgreementPricingProgram.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.AgreementPricingProgram.RelationType, value);
			}
		}

		virtual public bool ExistAgreementPricingProgram
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.AgreementPricingProgram.RelationType);
			}
		}

		virtual public void RemoveAgreementPricingProgram()
		{
			Strategy.RemoveCompositeRole(Meta.AgreementPricingProgram.RelationType);
		}


		virtual public global::System.String Description 
		{
			get
			{
				return (global::System.String) Strategy.GetUnitRole(Meta.Description.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Description.RelationType, value);
			}
		}

		virtual public bool ExistDescription{
			get
			{
				return Strategy.ExistUnitRole(Meta.Description.RelationType);
			}
		}

		virtual public void RemoveDescription()
		{
			Strategy.RemoveUnitRole(Meta.Description.RelationType);
		}


		virtual public Currency Currency
		{ 
			get
			{
				return (Currency) Strategy.GetCompositeRole(Meta.Currency.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.Currency.RelationType, value);
			}
		}

		virtual public bool ExistCurrency
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.Currency.RelationType);
			}
		}

		virtual public void RemoveCurrency()
		{
			Strategy.RemoveCompositeRole(Meta.Currency.RelationType);
		}


		virtual public OrderKind OrderKind
		{ 
			get
			{
				return (OrderKind) Strategy.GetCompositeRole(Meta.OrderKind.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.OrderKind.RelationType, value);
			}
		}

		virtual public bool ExistOrderKind
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.OrderKind.RelationType);
			}
		}

		virtual public void RemoveOrderKind()
		{
			Strategy.RemoveCompositeRole(Meta.OrderKind.RelationType);
		}


		virtual public OrderValue OrderValue
		{ 
			get
			{
				return (OrderValue) Strategy.GetCompositeRole(Meta.OrderValue.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.OrderValue.RelationType, value);
			}
		}

		virtual public bool ExistOrderValue
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.OrderValue.RelationType);
			}
		}

		virtual public void RemoveOrderValue()
		{
			Strategy.RemoveCompositeRole(Meta.OrderValue.RelationType);
		}


		virtual public global::System.Decimal? Price 
		{
			get
			{
				return (global::System.Decimal?) Strategy.GetUnitRole(Meta.Price.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Price.RelationType, value);
			}
		}

		virtual public bool ExistPrice{
			get
			{
				return Strategy.ExistUnitRole(Meta.Price.RelationType);
			}
		}

		virtual public void RemovePrice()
		{
			Strategy.RemoveUnitRole(Meta.Price.RelationType);
		}


		virtual public ProductCategory ProductCategory
		{ 
			get
			{
				return (ProductCategory) Strategy.GetCompositeRole(Meta.ProductCategory.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.ProductCategory.RelationType, value);
			}
		}

		virtual public bool ExistProductCategory
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.ProductCategory.RelationType);
			}
		}

		virtual public void RemoveProductCategory()
		{
			Strategy.RemoveCompositeRole(Meta.ProductCategory.RelationType);
		}


		virtual public SalesChannel SalesChannel
		{ 
			get
			{
				return (SalesChannel) Strategy.GetCompositeRole(Meta.SalesChannel.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.SalesChannel.RelationType, value);
			}
		}

		virtual public bool ExistSalesChannel
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.SalesChannel.RelationType);
			}
		}

		virtual public void RemoveSalesChannel()
		{
			Strategy.RemoveCompositeRole(Meta.SalesChannel.RelationType);
		}


		virtual public global::System.DateTime FromDate 
		{
			get
			{
				return (global::System.DateTime) Strategy.GetUnitRole(Meta.FromDate.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.FromDate.RelationType, value);
			}
		}

		virtual public bool ExistFromDate{
			get
			{
				return Strategy.ExistUnitRole(Meta.FromDate.RelationType);
			}
		}

		virtual public void RemoveFromDate()
		{
			Strategy.RemoveUnitRole(Meta.FromDate.RelationType);
		}


		virtual public global::System.DateTime? ThroughDate 
		{
			get
			{
				return (global::System.DateTime?) Strategy.GetUnitRole(Meta.ThroughDate.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.ThroughDate.RelationType, value);
			}
		}

		virtual public bool ExistThroughDate{
			get
			{
				return Strategy.ExistUnitRole(Meta.ThroughDate.RelationType);
			}
		}

		virtual public void RemoveThroughDate()
		{
			Strategy.RemoveUnitRole(Meta.ThroughDate.RelationType);
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


		virtual public global::System.String Comment 
		{
			get
			{
				return (global::System.String) Strategy.GetUnitRole(Meta.Comment.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Comment.RelationType, value);
			}
		}

		virtual public bool ExistComment{
			get
			{
				return Strategy.ExistUnitRole(Meta.Comment.RelationType);
			}
		}

		virtual public void RemoveComment()
		{
			Strategy.RemoveUnitRole(Meta.Comment.RelationType);
		}



		virtual public EngagementRate EngagementRateWhereGoverningPriceComponent
		{ 
			get
			{
				return (EngagementRate) Strategy.GetCompositeAssociation(Meta.EngagementRateWhereGoverningPriceComponent.RelationType);
			}
		} 

		virtual public bool ExistEngagementRateWhereGoverningPriceComponent
		{
			get
			{
				return Strategy.ExistCompositeAssociation(Meta.EngagementRateWhereGoverningPriceComponent.RelationType);
			}
		}


		virtual public global::Allors.Extent<PriceableVersion> PriceableVersionsWhereCurrentPriceComponent
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.PriceableVersionsWhereCurrentPriceComponent.RelationType);
			}
		}

		virtual public bool ExistPriceableVersionsWhereCurrentPriceComponent
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.PriceableVersionsWhereCurrentPriceComponent.RelationType);
			}
		}


		virtual public Part PartWherePriceComponent
		{ 
			get
			{
				return (Part) Strategy.GetCompositeAssociation(Meta.PartWherePriceComponent.RelationType);
			}
		} 

		virtual public bool ExistPartWherePriceComponent
		{
			get
			{
				return Strategy.ExistCompositeAssociation(Meta.PartWherePriceComponent.RelationType);
			}
		}


		virtual public global::Allors.Extent<Priceable> PriceablesWhereCurrentPriceComponent
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.PriceablesWhereCurrentPriceComponent.RelationType);
			}
		}

		virtual public bool ExistPriceablesWhereCurrentPriceComponent
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.PriceablesWhereCurrentPriceComponent.RelationType);
			}
		}


		virtual public Product ProductWhereVirtualProductPriceComponent
		{ 
			get
			{
				return (Product) Strategy.GetCompositeAssociation(Meta.ProductWhereVirtualProductPriceComponent.RelationType);
			}
		} 

		virtual public bool ExistProductWhereVirtualProductPriceComponent
		{
			get
			{
				return Strategy.ExistCompositeAssociation(Meta.ProductWhereVirtualProductPriceComponent.RelationType);
			}
		}


		virtual public global::Allors.Extent<Product> ProductsWhereBasePrice
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductsWhereBasePrice.RelationType);
			}
		}

		virtual public bool ExistProductsWhereBasePrice
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductsWhereBasePrice.RelationType);
			}
		}


		virtual public global::Allors.Extent<ProductFeature> ProductFeaturesWhereBasePrice
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductFeaturesWhereBasePrice.RelationType);
			}
		}

		virtual public bool ExistProductFeaturesWhereBasePrice
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductFeaturesWhereBasePrice.RelationType);
			}
		}



		public ObjectOnBuild OnBuild()
		{ 
			var method = new SurchargeComponentOnBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnBuild OnBuild(System.Action<ObjectOnBuild> action)
		{ 
			var method = new SurchargeComponentOnBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild()
		{ 
			var method = new SurchargeComponentOnPostBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild(System.Action<ObjectOnPostBuild> action)
		{ 
			var method = new SurchargeComponentOnPostBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive()
		{ 
			var method = new SurchargeComponentOnPreDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive(System.Action<ObjectOnPreDerive> action)
		{ 
			var method = new SurchargeComponentOnPreDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive()
		{ 
			var method = new SurchargeComponentOnDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive(System.Action<ObjectOnDerive> action)
		{ 
			var method = new SurchargeComponentOnDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive()
		{ 
			var method = new SurchargeComponentOnPostDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive(System.Action<ObjectOnPostDerive> action)
		{ 
			var method = new SurchargeComponentOnPostDerive(this);
            action(method);
            method.Execute();
            return method;
		}
	}

	public partial class SurchargeComponentBuilder : Allors.ObjectBuilder<SurchargeComponent> , PriceComponentBuilder, global::System.IDisposable
	{		
		public SurchargeComponentBuilder(Allors.ISession session) : base(session)
	    {
	    }

		protected override void OnBuild(SurchargeComponent instance)
		{
			

			if(this.Percentage.HasValue)
			{
				instance.Percentage = this.Percentage.Value;
			}			
		
		
			

			if(this.Rate.HasValue)
			{
				instance.Rate = this.Rate.Value;
			}			
		
		

			instance.Description = this.Description;
		
		
			

			if(this.Price.HasValue)
			{
				instance.Price = this.Price.Value;
			}			
		
		
			

			if(this.FromDate.HasValue)
			{
				instance.FromDate = this.FromDate.Value;
			}			
		
		
			

			if(this.ThroughDate.HasValue)
			{
				instance.ThroughDate = this.ThroughDate.Value;
			}			
		
		

			instance.Comment = this.Comment;
		
		

			instance.GeographicBoundary = this.GeographicBoundary;

		

			instance.RevenueValueBreak = this.RevenueValueBreak;

		

			instance.PartyClassification = this.PartyClassification;

		

			instance.OrderQuantityBreak = this.OrderQuantityBreak;

		

			instance.PackageQuantityBreak = this.PackageQuantityBreak;

		

			instance.Product = this.Product;

		

			instance.RevenueQuantityBreak = this.RevenueQuantityBreak;

		

			instance.ProductFeature = this.ProductFeature;

		

			instance.AgreementPricingProgram = this.AgreementPricingProgram;

				

			instance.OrderKind = this.OrderKind;

		

			instance.OrderValue = this.OrderValue;

		

			instance.ProductCategory = this.ProductCategory;

		

			instance.SalesChannel = this.SalesChannel;

		
			if(this.DeniedPermissions!=null)
			{
				instance.DeniedPermissions = this.DeniedPermissions.ToArray();
			}
		
			if(this.SecurityTokens!=null)
			{
				instance.SecurityTokens = this.SecurityTokens.ToArray();
			}
		
		}


				public global::System.Decimal? Percentage {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithPercentage(global::System.Decimal? value)
		        {
				    if(this.Percentage!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Percentage = value;
		            return this;
		        }	

				public GeographicBoundary GeographicBoundary {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithGeographicBoundary(GeographicBoundary value)
		        {
		            if(this.GeographicBoundary!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.GeographicBoundary = value;
		            return this;
		        }		

				
				public global::System.Decimal? Rate {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithRate(global::System.Decimal? value)
		        {
				    if(this.Rate!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Rate = value;
		            return this;
		        }	

				public RevenueValueBreak RevenueValueBreak {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithRevenueValueBreak(RevenueValueBreak value)
		        {
		            if(this.RevenueValueBreak!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.RevenueValueBreak = value;
		            return this;
		        }		

				
				public PartyClassification PartyClassification {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithPartyClassification(PartyClassification value)
		        {
		            if(this.PartyClassification!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.PartyClassification = value;
		            return this;
		        }		

				
				public OrderQuantityBreak OrderQuantityBreak {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithOrderQuantityBreak(OrderQuantityBreak value)
		        {
		            if(this.OrderQuantityBreak!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.OrderQuantityBreak = value;
		            return this;
		        }		

				
				public PackageQuantityBreak PackageQuantityBreak {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithPackageQuantityBreak(PackageQuantityBreak value)
		        {
		            if(this.PackageQuantityBreak!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.PackageQuantityBreak = value;
		            return this;
		        }		

				
				public Product Product {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithProduct(Product value)
		        {
		            if(this.Product!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.Product = value;
		            return this;
		        }		

				
				public RevenueQuantityBreak RevenueQuantityBreak {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithRevenueQuantityBreak(RevenueQuantityBreak value)
		        {
		            if(this.RevenueQuantityBreak!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.RevenueQuantityBreak = value;
		            return this;
		        }		

				
				public ProductFeature ProductFeature {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithProductFeature(ProductFeature value)
		        {
		            if(this.ProductFeature!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.ProductFeature = value;
		            return this;
		        }		

				
				public AgreementPricingProgram AgreementPricingProgram {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithAgreementPricingProgram(AgreementPricingProgram value)
		        {
		            if(this.AgreementPricingProgram!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.AgreementPricingProgram = value;
		            return this;
		        }		

				
				public global::System.String Description {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithDescription(global::System.String value)
		        {
				    if(this.Description!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Description = value;
		            return this;
		        }	

				public OrderKind OrderKind {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithOrderKind(OrderKind value)
		        {
		            if(this.OrderKind!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.OrderKind = value;
		            return this;
		        }		

				
				public OrderValue OrderValue {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithOrderValue(OrderValue value)
		        {
		            if(this.OrderValue!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.OrderValue = value;
		            return this;
		        }		

				
				public global::System.Decimal? Price {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithPrice(global::System.Decimal? value)
		        {
				    if(this.Price!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Price = value;
		            return this;
		        }	

				public ProductCategory ProductCategory {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithProductCategory(ProductCategory value)
		        {
		            if(this.ProductCategory!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.ProductCategory = value;
		            return this;
		        }		

				
				public SalesChannel SalesChannel {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithSalesChannel(SalesChannel value)
		        {
		            if(this.SalesChannel!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.SalesChannel = value;
		            return this;
		        }		

				
				public global::System.DateTime? FromDate {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithFromDate(global::System.DateTime? value)
		        {
				    if(this.FromDate!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.FromDate = value;
		            return this;
		        }	

				public global::System.DateTime? ThroughDate {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithThroughDate(global::System.DateTime? value)
		        {
				    if(this.ThroughDate!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.ThroughDate = value;
		            return this;
		        }	

				public global::System.Collections.Generic.List<Permission> DeniedPermissions {get; set;}	

				/// <exclude/>
				public SurchargeComponentBuilder WithDeniedPermission(Permission value)
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
				public SurchargeComponentBuilder WithSecurityToken(SecurityToken value)
		        {
					if(this.SecurityTokens == null)
					{
						this.SecurityTokens = new global::System.Collections.Generic.List<SecurityToken>(); 
					}
		            this.SecurityTokens.Add(value);
		            return this;
		        }		

				
				public global::System.String Comment {get; set;}

				/// <exclude/>
				public SurchargeComponentBuilder WithComment(global::System.String value)
		        {
				    if(this.Comment!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Comment = value;
		            return this;
		        }	


	}

	public partial class SurchargeComponents : global::Allors.ObjectsBase<SurchargeComponent>
	{
		public SurchargeComponents(Allors.ISession session) : base(session)
		{
		}

		public Allors.Meta.MetaSurchargeComponent Meta
		{
			get
			{
				return Allors.Meta.MetaSurchargeComponent.Instance;
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