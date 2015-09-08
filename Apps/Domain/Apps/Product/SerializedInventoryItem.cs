// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializedInventoryItem.cs" company="Allors bvba">
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
    public partial class SerializedInventoryItem
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new SerializedInventoryItemObjectStates(this.Strategy.Session).Good;
            }

            if (!this.ExistFacility)
            {
                if (Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation != null)
                {
                    this.Facility = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation.DefaultFacility;
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Log.AssertAtLeastOne(this, SerializedInventoryItems.Meta.Good, SerializedInventoryItems.Meta.Part);
            derivation.Log.AssertExistsAtMostOne(this, SerializedInventoryItems.Meta.Good, SerializedInventoryItems.Meta.Part);

            this.DeriveCurrentObjectState(derivation);
        }

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                SerializedInventoryItemStatus currentStatus = new SerializedInventoryItemStatusBuilder(this.Strategy.Session).WithSerializedInventoryItemObjectState(this.CurrentObjectState).Build();
                this.AddInventoryItemStatus(currentStatus);
                this.CurrentInventoryItemStatus = currentStatus;
            }

            if (this.ExistCurrentObjectState)
            {
                this.CurrentObjectState.Process(this);
            }

            this.DeriveProductCategories(derivation);
        }

        public void AppsOnDeriveProductCategories(IDerivation derivation)
        {
            this.RemoveDerivedProductCategories();

            if (this.ExistGood)
            {
                foreach (ProductCategory productCategory in this.Good.ProductCategories)
                {
                    this.AddDerivedProductCategory(productCategory);
                    this.AddParentCategories(productCategory);
                }
            }
        }

        private void AddParentCategories(ProductCategory productCategory)
        {
            if (productCategory.ExistParents)
            {
                foreach (ProductCategory parent in productCategory.Parents)
                {
                    this.AddDerivedProductCategory(parent);
                    this.AddParentCategories(parent);
                }
            }
        }
    }
}