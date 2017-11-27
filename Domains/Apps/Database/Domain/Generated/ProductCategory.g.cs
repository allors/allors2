// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Domain
{
	public partial class ProductCategory : Allors.IObject , AccessControlledObject, UniquelyIdentifiable, Deletable
	{
		private readonly IStrategy strategy;

		public ProductCategory(Allors.IStrategy strategy) 
		{
			this.strategy = strategy;
		}

		public Allors.Meta.MetaProductCategory Meta
		{
			get
			{
				return Allors.Meta.MetaProductCategory.Instance;
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

		public static ProductCategory Instantiate (Allors.ISession allorsSession, string allorsObjectId)
		{
			return (ProductCategory) allorsSession.Instantiate(allorsObjectId);		
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



		virtual public Package Package
		{ 
			get
			{
				return (Package) Strategy.GetCompositeRole(Meta.Package.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.Package.RelationType, value);
			}
		}

		virtual public bool ExistPackage
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.Package.RelationType);
			}
		}

		virtual public void RemovePackage()
		{
			Strategy.RemoveCompositeRole(Meta.Package.RelationType);
		}


		virtual public global::System.String Code 
		{
			get
			{
				return (global::System.String) Strategy.GetUnitRole(Meta.Code.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.Code.RelationType, value);
			}
		}

		virtual public bool ExistCode{
			get
			{
				return Strategy.ExistUnitRole(Meta.Code.RelationType);
			}
		}

		virtual public void RemoveCode()
		{
			Strategy.RemoveUnitRole(Meta.Code.RelationType);
		}


		virtual public global::Allors.Extent<ProductCategory> Parents
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.Parents.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.Parents.RelationType, value);
			}
		}

		virtual public void AddParent (ProductCategory value)
		{
			Strategy.AddCompositeRole(Meta.Parents.RelationType, value);
		}

		virtual public void RemoveParent (ProductCategory value)
		{
			Strategy.RemoveCompositeRole(Meta.Parents.RelationType, value);
		}

		virtual public bool ExistParents
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.Parents.RelationType);
			}
		}

		virtual public void RemoveParents()
		{
			Strategy.RemoveCompositeRoles(Meta.Parents.RelationType);
		}


		virtual public global::Allors.Extent<ProductCategory> Children
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.Children.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.Children.RelationType, value);
			}
		}

		virtual public void AddChild (ProductCategory value)
		{
			Strategy.AddCompositeRole(Meta.Children.RelationType, value);
		}

		virtual public void RemoveChild (ProductCategory value)
		{
			Strategy.RemoveCompositeRole(Meta.Children.RelationType, value);
		}

		virtual public bool ExistChildren
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.Children.RelationType);
			}
		}

		virtual public void RemoveChildren()
		{
			Strategy.RemoveCompositeRoles(Meta.Children.RelationType);
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


		virtual public global::Allors.Extent<LocalisedText> LocalisedNames
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.LocalisedNames.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.LocalisedNames.RelationType, value);
			}
		}

		virtual public void AddLocalisedName (LocalisedText value)
		{
			Strategy.AddCompositeRole(Meta.LocalisedNames.RelationType, value);
		}

		virtual public void RemoveLocalisedName (LocalisedText value)
		{
			Strategy.RemoveCompositeRole(Meta.LocalisedNames.RelationType, value);
		}

		virtual public bool ExistLocalisedNames
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.LocalisedNames.RelationType);
			}
		}

		virtual public void RemoveLocalisedNames()
		{
			Strategy.RemoveCompositeRoles(Meta.LocalisedNames.RelationType);
		}


		virtual public global::Allors.Extent<LocalisedText> LocalisedDescriptions
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.LocalisedDescriptions.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.LocalisedDescriptions.RelationType, value);
			}
		}

		virtual public void AddLocalisedDescription (LocalisedText value)
		{
			Strategy.AddCompositeRole(Meta.LocalisedDescriptions.RelationType, value);
		}

		virtual public void RemoveLocalisedDescription (LocalisedText value)
		{
			Strategy.RemoveCompositeRole(Meta.LocalisedDescriptions.RelationType, value);
		}

		virtual public bool ExistLocalisedDescriptions
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.LocalisedDescriptions.RelationType);
			}
		}

		virtual public void RemoveLocalisedDescriptions()
		{
			Strategy.RemoveCompositeRoles(Meta.LocalisedDescriptions.RelationType);
		}


		virtual public Media CategoryImage
		{ 
			get
			{
				return (Media) Strategy.GetCompositeRole(Meta.CategoryImage.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.CategoryImage.RelationType, value);
			}
		}

		virtual public bool ExistCategoryImage
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.CategoryImage.RelationType);
			}
		}

		virtual public void RemoveCategoryImage()
		{
			Strategy.RemoveCompositeRole(Meta.CategoryImage.RelationType);
		}


		virtual public global::Allors.Extent<ProductCategory> SuperJacent
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.SuperJacent.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.SuperJacent.RelationType, value);
			}
		}

		virtual public void AddSuperJacent (ProductCategory value)
		{
			Strategy.AddCompositeRole(Meta.SuperJacent.RelationType, value);
		}

		virtual public void RemoveSuperJacent (ProductCategory value)
		{
			Strategy.RemoveCompositeRole(Meta.SuperJacent.RelationType, value);
		}

		virtual public bool ExistSuperJacent
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.SuperJacent.RelationType);
			}
		}

		virtual public void RemoveSuperJacent()
		{
			Strategy.RemoveCompositeRoles(Meta.SuperJacent.RelationType);
		}


		virtual public CatScope CatScope
		{ 
			get
			{
				return (CatScope) Strategy.GetCompositeRole(Meta.CatScope.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.CatScope.RelationType, value);
			}
		}

		virtual public bool ExistCatScope
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.CatScope.RelationType);
			}
		}

		virtual public void RemoveCatScope()
		{
			Strategy.RemoveCompositeRole(Meta.CatScope.RelationType);
		}


		virtual public global::Allors.Extent<Product> AllProducts
		{ 
			get
			{
				return Strategy.GetCompositeRoles(Meta.AllProducts.RelationType);
			}
			set
			{
				Strategy.SetCompositeRoles(Meta.AllProducts.RelationType, value);
			}
		}

		virtual public void AddAllProduct (Product value)
		{
			Strategy.AddCompositeRole(Meta.AllProducts.RelationType, value);
		}

		virtual public void RemoveAllProduct (Product value)
		{
			Strategy.RemoveCompositeRole(Meta.AllProducts.RelationType, value);
		}

		virtual public bool ExistAllProducts
		{
			get
			{
				return Strategy.ExistCompositeRoles(Meta.AllProducts.RelationType);
			}
		}

		virtual public void RemoveAllProducts()
		{
			Strategy.RemoveCompositeRoles(Meta.AllProducts.RelationType);
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



		virtual public global::Allors.Extent<Brand> BrandsWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.BrandsWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistBrandsWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.BrandsWhereProductCategory.RelationType);
			}
		}


		virtual public Catalogue CatalogueWhereProductCategory
		{ 
			get
			{
				return (Catalogue) Strategy.GetCompositeAssociation(Meta.CatalogueWhereProductCategory.RelationType);
			}
		} 

		virtual public bool ExistCatalogueWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociation(Meta.CatalogueWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<OrganisationGlAccount> OrganisationGlAccountsWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.OrganisationGlAccountsWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistOrganisationGlAccountsWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.OrganisationGlAccountsWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<PartyProductCategoryRevenue> PartyProductCategoryRevenuesWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.PartyProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistPartyProductCategoryRevenuesWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.PartyProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<ProductCategory> ProductCategoriesWhereParent
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductCategoriesWhereParent.RelationType);
			}
		}

		virtual public bool ExistProductCategoriesWhereParent
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductCategoriesWhereParent.RelationType);
			}
		}


		virtual public global::Allors.Extent<ProductCategory> ProductCategoriesWhereChild
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductCategoriesWhereChild.RelationType);
			}
		}

		virtual public bool ExistProductCategoriesWhereChild
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductCategoriesWhereChild.RelationType);
			}
		}


		virtual public global::Allors.Extent<ProductCategory> ProductCategoriesWhereSuperJacent
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductCategoriesWhereSuperJacent.RelationType);
			}
		}

		virtual public bool ExistProductCategoriesWhereSuperJacent
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductCategoriesWhereSuperJacent.RelationType);
			}
		}


		virtual public global::Allors.Extent<ProductCategoryRevenue> ProductCategoryRevenuesWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistProductCategoryRevenuesWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<RevenueQuantityBreak> RevenueQuantityBreaksWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.RevenueQuantityBreaksWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistRevenueQuantityBreaksWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.RevenueQuantityBreaksWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<RevenueValueBreak> RevenueValueBreaksWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.RevenueValueBreaksWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistRevenueValueBreaksWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.RevenueValueBreaksWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<SalesRepPartyProductCategoryRevenue> SalesRepPartyProductCategoryRevenuesWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.SalesRepPartyProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistSalesRepPartyProductCategoryRevenuesWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.SalesRepPartyProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<SalesRepProductCategoryRevenue> SalesRepProductCategoryRevenuesWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.SalesRepProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistSalesRepProductCategoryRevenuesWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.SalesRepProductCategoryRevenuesWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<SalesRepRelationship> SalesRepRelationshipsWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.SalesRepRelationshipsWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistSalesRepRelationshipsWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.SalesRepRelationshipsWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<InventoryItemVersion> InventoryItemVersionsWhereDerivedProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.InventoryItemVersionsWhereDerivedProductCategory.RelationType);
			}
		}

		virtual public bool ExistInventoryItemVersionsWhereDerivedProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.InventoryItemVersionsWhereDerivedProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<InventoryItem> InventoryItemsWhereDerivedProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.InventoryItemsWhereDerivedProductCategory.RelationType);
			}
		}

		virtual public bool ExistInventoryItemsWhereDerivedProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.InventoryItemsWhereDerivedProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<PriceComponent> PriceComponentsWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.PriceComponentsWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistPriceComponentsWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.PriceComponentsWhereProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<Product> ProductsWherePrimaryProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductsWherePrimaryProductCategory.RelationType);
			}
		}

		virtual public bool ExistProductsWherePrimaryProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductsWherePrimaryProductCategory.RelationType);
			}
		}


		virtual public global::Allors.Extent<Product> ProductsWhereProductCategoriesExpanded
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductsWhereProductCategoriesExpanded.RelationType);
			}
		}

		virtual public bool ExistProductsWhereProductCategoriesExpanded
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductsWhereProductCategoriesExpanded.RelationType);
			}
		}


		virtual public global::Allors.Extent<Product> ProductsWhereProductCategory
		{ 
			get
			{
				return Strategy.GetCompositeAssociations(Meta.ProductsWhereProductCategory.RelationType);
			}
		}

		virtual public bool ExistProductsWhereProductCategory
		{
			get
			{
				return Strategy.ExistCompositeAssociations(Meta.ProductsWhereProductCategory.RelationType);
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
			var method = new ProductCategoryOnBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnBuild OnBuild(System.Action<ObjectOnBuild> action)
		{ 
			var method = new ProductCategoryOnBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild()
		{ 
			var method = new ProductCategoryOnPostBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild(System.Action<ObjectOnPostBuild> action)
		{ 
			var method = new ProductCategoryOnPostBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive()
		{ 
			var method = new ProductCategoryOnPreDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive(System.Action<ObjectOnPreDerive> action)
		{ 
			var method = new ProductCategoryOnPreDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive()
		{ 
			var method = new ProductCategoryOnDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive(System.Action<ObjectOnDerive> action)
		{ 
			var method = new ProductCategoryOnDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive()
		{ 
			var method = new ProductCategoryOnPostDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive(System.Action<ObjectOnPostDerive> action)
		{ 
			var method = new ProductCategoryOnPostDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public DeletableDelete Delete()
		{ 
			var method = new ProductCategoryDelete(this);
            method.Execute();
            return method;
		}

		public DeletableDelete Delete(System.Action<DeletableDelete> action)
		{ 
			var method = new ProductCategoryDelete(this);
            action(method);
            method.Execute();
            return method;
		}
	}

	public partial class ProductCategoryBuilder : Allors.ObjectBuilder<ProductCategory> , AccessControlledObjectBuilder, UniquelyIdentifiableBuilder, DeletableBuilder, global::System.IDisposable
	{		
		public ProductCategoryBuilder(Allors.ISession session) : base(session)
	    {
	    }

		protected override void OnBuild(ProductCategory instance)
		{

			instance.Code = this.Code;
		
						
			

			if(this.UniqueId.HasValue)
			{
				instance.UniqueId = this.UniqueId.Value;
			}			
		
		

			instance.Package = this.Package;

		
			if(this.Parents!=null)
			{
				instance.Parents = this.Parents.ToArray();
			}
				
			if(this.LocalisedNames!=null)
			{
				instance.LocalisedNames = this.LocalisedNames.ToArray();
			}
		
			if(this.LocalisedDescriptions!=null)
			{
				instance.LocalisedDescriptions = this.LocalisedDescriptions.ToArray();
			}
		

			instance.CategoryImage = this.CategoryImage;

				

			instance.CatScope = this.CatScope;

				
			if(this.DeniedPermissions!=null)
			{
				instance.DeniedPermissions = this.DeniedPermissions.ToArray();
			}
		
			if(this.SecurityTokens!=null)
			{
				instance.SecurityTokens = this.SecurityTokens.ToArray();
			}
		
		}


				public Package Package {get; set;}

				/// <exclude/>
				public ProductCategoryBuilder WithPackage(Package value)
		        {
		            if(this.Package!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.Package = value;
		            return this;
		        }		

				
				public global::System.String Code {get; set;}

				/// <exclude/>
				public ProductCategoryBuilder WithCode(global::System.String value)
		        {
				    if(this.Code!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.Code = value;
		            return this;
		        }	

				public global::System.Collections.Generic.List<ProductCategory> Parents {get; set;}	

				/// <exclude/>
				public ProductCategoryBuilder WithParent(ProductCategory value)
		        {
					if(this.Parents == null)
					{
						this.Parents = new global::System.Collections.Generic.List<ProductCategory>(); 
					}
		            this.Parents.Add(value);
		            return this;
		        }		

				
				public global::System.Collections.Generic.List<LocalisedText> LocalisedNames {get; set;}	

				/// <exclude/>
				public ProductCategoryBuilder WithLocalisedName(LocalisedText value)
		        {
					if(this.LocalisedNames == null)
					{
						this.LocalisedNames = new global::System.Collections.Generic.List<LocalisedText>(); 
					}
		            this.LocalisedNames.Add(value);
		            return this;
		        }		

				
				public global::System.Collections.Generic.List<LocalisedText> LocalisedDescriptions {get; set;}	

				/// <exclude/>
				public ProductCategoryBuilder WithLocalisedDescription(LocalisedText value)
		        {
					if(this.LocalisedDescriptions == null)
					{
						this.LocalisedDescriptions = new global::System.Collections.Generic.List<LocalisedText>(); 
					}
		            this.LocalisedDescriptions.Add(value);
		            return this;
		        }		

				
				public Media CategoryImage {get; set;}

				/// <exclude/>
				public ProductCategoryBuilder WithCategoryImage(Media value)
		        {
		            if(this.CategoryImage!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.CategoryImage = value;
		            return this;
		        }		

				
				public CatScope CatScope {get; set;}

				/// <exclude/>
				public ProductCategoryBuilder WithCatScope(CatScope value)
		        {
		            if(this.CatScope!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.CatScope = value;
		            return this;
		        }		

				
				public global::System.Collections.Generic.List<Permission> DeniedPermissions {get; set;}	

				/// <exclude/>
				public ProductCategoryBuilder WithDeniedPermission(Permission value)
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
				public ProductCategoryBuilder WithSecurityToken(SecurityToken value)
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
				public ProductCategoryBuilder WithUniqueId(global::System.Guid? value)
		        {
				    if(this.UniqueId!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.UniqueId = value;
		            return this;
		        }	


	}

	public partial class ProductCategories : global::Allors.ObjectsBase<ProductCategory>
	{
		public ProductCategories(Allors.ISession session) : base(session)
		{
		}

		public Allors.Meta.MetaProductCategory Meta
		{
			get
			{
				return Allors.Meta.MetaProductCategory.Instance;
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