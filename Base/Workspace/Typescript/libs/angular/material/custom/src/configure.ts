import { MetaPopulation } from '@allors/meta/system';
import { Meta } from '@allors/meta/generated';

import '@allors/meta/core';
import { FilterDefinition, SearchFactory } from '@allors/angular/core';
import { And, Like, ContainedIn, Extent, Equals, Exists, Contains } from '@allors/data/system';
import { Sorter } from '@allors/angular/material/core';
import {
  Organisation,
  Country,
  ProductCategory,
  ProductIdentification,
  Brand,
  Model,
  InventoryItemKind,
  ProductType,
  Facility,
} from '@allors/domain/generated';
import { InternalOrganisationId, Filters } from '@allors/angular/base';

export function configure(metaPopulation: MetaPopulation, internalOrganisationId: InternalOrganisationId) {
  const m = metaPopulation as Meta;

  const productTypeSearch = new SearchFactory({
    objectType: m.ProductType,
    roleTypes: [m.ProductType.Name],
  });

  const inventoryItemKindSearch = new SearchFactory({
    objectType: m.InventoryItemKind,
    predicates: [new Equals({ propertyType: m.Enumeration.IsActive, value: true })],
    roleTypes: [m.InventoryItemKind.Name],
  });

  const manufacturerSearch = new SearchFactory({
    objectType: m.Organisation,
    predicates: [new Equals({ propertyType: m.Organisation.IsManufacturer, value: true })],
    roleTypes: [m.Organisation.PartyName],
  });

  const facilitySearch = new SearchFactory({
    objectType: m.Facility,
    roleTypes: [m.Facility.Name],
  });

  const countrySearch = new SearchFactory({
    objectType: m.Country,
    roleTypes: [m.Country.Name],
  });

  const internalOrganisationSearch = new SearchFactory({
    objectType: m.Organisation,
    roleTypes: [m.Organisation.Name],
    post: (predicate: And) => {
      predicate.operands.push(new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }));
    },
  });

  const modelSearch = new SearchFactory({
    objectType: m.Model,
    roleTypes: [m.Model.Name],
  });

  const brandSearch = new SearchFactory({
    objectType: m.Brand,
    roleTypes: [m.Brand.Name],
  });

  const categorySearch = new SearchFactory({
    objectType: m.ProductCategory,
    roleTypes: [m.ProductCategory.DisplayName],
  });

  const productIdSearch = new SearchFactory({
    objectType: m.ProductIdentification,
    roleTypes: [m.ProductIdentification.Identification],
  });

  m.Person.list = '/contacts/people';
  m.Person.overview = '/contacts/person/:id';
  m.Person.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      new Like({ roleType: m.Person.LastName, parameter: 'lastName' }),
      new ContainedIn({
        propertyType: m.Party.PartyContactMechanisms,
        extent: new Extent({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Extent({
              objectType: m.PostalAddress,
              predicate: new ContainedIn({
                propertyType: m.PostalAddress.Country,
                parameter: 'country',
              }),
            }),
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.Party.PartyContactMechanisms,
        extent: new Extent({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Extent({
              objectType: m.PostalAddress,
              predicate: new Like({
                roleType: m.PostalAddress.Locality,
                parameter: 'city',
              }),
            }),
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.Party.CustomerRelationshipsWhereCustomer,
        extent: new Extent({
          objectType: m.CustomerRelationship,
          predicate: new Equals({
            propertyType: m.CustomerRelationship.InternalOrganisation,
            parameter: 'customerAt',
          }),
        }),
      }),
    ]),
    {
      customerAt: {
        search: () => internalOrganisationSearch,
        display: (v: Organisation) => v && v.Name,
      },
      country: { search:() => countrySearch, display: (v: Country) => v && v.Name },
    }
  );
  m.Person.sorter = new Sorter({
    name: [m.Person.FirstName, m.Person.LastName],
    lastModifiedDate: m.Person.LastModifiedDate,
  });

  m.Organisation.list = '/contacts/organisations';
  m.Organisation.overview = '/contacts/organisation/:id';
  m.Organisation.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.Organisation.Name, parameter: 'name' }),
      new ContainedIn({
        propertyType: m.Organisation.SupplierRelationshipsWhereSupplier,
        extent: new Extent({
          objectType: m.SupplierRelationship,
          predicate: new Equals({
            propertyType: m.SupplierRelationship.InternalOrganisation,
            parameter: 'supplierFor',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.Party.CustomerRelationshipsWhereCustomer,
        extent: new Extent({
          objectType: m.CustomerRelationship,
          predicate: new Equals({
            propertyType: m.CustomerRelationship.InternalOrganisation,
            parameter: 'customerAt',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.Party.PartyContactMechanisms,
        extent: new Extent({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Extent({
              objectType: m.PostalAddress,
              predicate: new ContainedIn({
                propertyType: m.PostalAddress.Country,
                parameter: 'country',
              }),
            }),
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.Party.PartyContactMechanisms,
        extent: new Extent({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Extent({
              objectType: m.PostalAddress,
              predicate: new Like({
                roleType: m.PostalAddress.Locality,
                parameter: 'city',
              }),
            }),
          }),
        }),
      }),
    ]),
    {
      customerAt: {
        search: () => internalOrganisationSearch,
        initialValue: () => internalOrganisationId.value,
        display: (v: Organisation) => v && v.Name,
      },
      supplierFor: {
        search: () => internalOrganisationSearch,
        initialValue: () => internalOrganisationId.value,
        display: (v: Organisation) => v && v.Name,
      },
      country: {
        search: () => countrySearch,
        display: (v: Country) => v && v.Name,
      },
    }
  );
  m.Organisation.sorter = new Sorter({
    name: m.Organisation.Name,
    lastModifiedDate: m.Organisation.LastModifiedDate,
  });

  m.CommunicationEvent.list = '/contacts/communicationevents';
  m.CommunicationEvent.filterDefinition = new FilterDefinition(
    new And([new Like({ roleType: m.CommunicationEvent.Subject, parameter: 'subject' })])
  );
  m.CommunicationEvent.sorter = new Sorter({
    subject: m.CommunicationEvent.Subject,
    lastModifiedDate: m.CommunicationEvent.LastModifiedDate,
  });

  m.EmailCommunication.overview = '/contacts/emailcommunication/:id';
  m.FaceToFaceCommunication.overview = '/contacts/facetofacecommunication/:id';
  m.LetterCorrespondence.overview = '/contacts/lettercorrespondence/:id';
  m.PhoneCommunication.overview = '/contacts/phonecommunication/:id';

  m.RequestForQuote.list = '/sales/requestsforquote';
  m.RequestForQuote.overview = '/sales/requestforquote/:id';
  m.ProductQuote.list = '/sales/productquotes';
  m.ProductQuote.overview = '/sales/productquote/:id';
  m.SalesOrder.list = '/sales/salesorders';
  m.SalesOrder.overview = '/sales/salesorder/:id';

  m.Good.list = '/products/goods';
  m.Good.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.Good.Name, parameter: 'name' }),
      new Like({ roleType: m.Good.Keywords, parameter: 'keyword' }),
      new Contains({ propertyType: m.Good.ProductCategoriesWhereProduct, parameter: 'category' }),
      new Contains({ propertyType: m.Good.ProductIdentifications, parameter: 'identification' }),
      new Exists({ propertyType: m.Good.SalesDiscontinuationDate, parameter: 'discontinued' }),
    ]),
    {
      category: { search: () => categorySearch, display: (v: ProductCategory) => v && v.Name },
      identification: { search:() =>  productIdSearch, display: (v: ProductIdentification) => v && v.Identification },
      brand: { search: () => brandSearch, display: (v: Brand) => v && v.Name },
      model: { search: () => modelSearch, display: (v: Model) => v && v.Name },
    }
  );
  m.Good.sorter = new Sorter({
    name: [m.Good.Name],
  });

  m.NonUnifiedGood.overview = '/products/nonunifiedgood/:id';
  m.NonUnifiedPart.overview = '/products/nonunifiedpart/:id';

  m.Part.list = '/products/parts';
  m.Part.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.Part.Name, parameter: 'name' }),
      new Like({ roleType: m.Part.Keywords, parameter: 'keyword' }),
      new Like({ roleType: m.Part.HsCode, parameter: 'hsCode' }),
      new Contains({ propertyType: m.Part.ProductIdentifications, parameter: 'identification' }),
      new Contains({ propertyType: m.Part.SuppliedBy, parameter: 'supplier' }),
      new Equals({ propertyType: m.Part.ManufacturedBy, parameter: 'manufacturer' }),
      new Equals({ propertyType: m.Part.Brand, parameter: 'brand' }),
      new Equals({ propertyType: m.Part.Model, parameter: 'model' }),
      new Equals({ propertyType: m.Part.InventoryItemKind, parameter: 'kind' }),
      new Equals({ propertyType: m.Part.ProductType, parameter: 'type' }),
      new ContainedIn({
        propertyType: m.Part.InventoryItemsWherePart,
        extent: new Extent({
          objectType: m.InventoryItem,
          predicate: new Equals({
            propertyType: m.InventoryItem.Facility,
            parameter: 'facility',
          }),
        }),
      }),
    ]),
    {
      supplier: {search: () => Filters.suppliersFilter(m, internalOrganisationId.value), display: (v: Organisation) => v && v.PartyName,
      },
      manufacturer: { search: () => manufacturerSearch, display: (v: Organisation) => v && v.PartyName },
      brand: { search: () =>  brandSearch, display: (v: Brand) => v && v && v.Name },
      model: { search: () => modelSearch, display: (v: Model) => v.Name },
      kind: { search: () => inventoryItemKindSearch, display: (v: InventoryItemKind) => v && v.Name },
      type: { search: () => productTypeSearch, display: (v: ProductType) => v && v.Name },
      identification: { search: () => productIdSearch, display: (v: ProductIdentification) => v && v.Identification },
      facility: { search: () => facilitySearch, display: (v: Facility) => v && v.Name },
    }
  );
  m.Part.sorter = new Sorter({
    name: m.NonUnifiedPart.Name,
  });

  m.Catalogue.list = '/products/catalogues';
  m.ProductCategory.list = '/products/productcategories';
  m.SerialisedItemCharacteristic.list = '/products/serialiseditemcharacteristics';
  m.ProductType.list = '/products/producttypes';
  m.SerialisedItem.list = '/products/serialiseditems';
  m.SerialisedItem.overview = '/products/serialisedItem/:id';

  m.UnifiedGood.list = '/products/unifiedgoods';
  m.UnifiedGood.overview = '/products/unifiedgood/:id';
  m.UnifiedGood.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.UnifiedGood.Name, parameter: 'name' }),
      new Like({ roleType: m.UnifiedGood.Keywords, parameter: 'keyword' }),
      new Contains({ propertyType: m.UnifiedGood.ProductCategoriesWhereProduct, parameter: 'category' }),
      new Contains({ propertyType: m.UnifiedGood.ProductIdentifications, parameter: 'identification' }),
      new Equals({ propertyType: m.UnifiedGood.Brand, parameter: 'brand' }),
      new Equals({ propertyType: m.UnifiedGood.Model, parameter: 'model' }),
      new Exists({ propertyType: m.UnifiedGood.SalesDiscontinuationDate, parameter: 'discontinued' }),
      new Exists({ propertyType: m.UnifiedGood.Photos, parameter: 'photos' }),
    ]),
    {
      category: { search: () => categorySearch, display: (v: ProductCategory) => v && v.DisplayName },
      identification: { search: () => productIdSearch, display: (v: ProductIdentification) => v && v.Identification },
      brand: { search: () => brandSearch, display: (v: Brand) => v && v.Name },
      model: { search: () => modelSearch, display: (v: Model) => v && v.Name },
    }
  );
  m.UnifiedGood.sorter = new Sorter({
    name: [m.UnifiedGood.Name],
    id: [m.UnifiedGood.ProductNumber],
    lastModifiedDate: m.UnifiedGood.LastModifiedDate,
  });

  m.SalesInvoice.list = '/sales/salesinvoices';
  m.SalesInvoice.overview = '/sales/salesinvoice/:id';

  m.PurchaseInvoice.list = '/purchasing/purchaseinvoices';
  m.PurchaseInvoice.overview = '/purchasing/purchaseinvoice/:id';
  m.PurchaseOrder.list = '/purchasing/purchaseorders';
  m.PurchaseOrder.overview = '/purchasing/purchaseorder/:id';

  m.Shipment.list = '/shipment/shipments';
  m.CustomerShipment.list = '/shipment/shipments';
  m.CustomerShipment.overview = '/shipment/customershipment/:id';
  m.PurchaseShipment.list = '/shipment/shipments';
  m.PurchaseShipment.overview = '/shipment/purchaseshipment/:id';
  m.Carrier.list = '/shipment/carriers';

  m.WorkEffort.list = '/workefforts/workefforts';
  m.WorkTask.overview = '/workefforts/worktask/:id';

  m.PositionType.list = '/humanresource/positiontypes';
  m.PositionTypeRate.list = '/humanresource/positiontyperates';

  m.TaskAssignment.list = '/workflow/taskassignments';
}
