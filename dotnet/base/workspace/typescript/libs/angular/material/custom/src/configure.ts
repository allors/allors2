import { MetaPopulation } from '@allors/meta/system';
import { Meta } from '@allors/meta/generated';

import '@allors/meta/core';
import { FilterDefinition, SearchFactory } from '@allors/angular/core';
import { And, Like, ContainedIn, Extent, Equals, Exists, Contains } from '@allors/data/system';
import { Sorter } from '@allors/angular/material/core';
import {
  Organisation,
  Country,
  Currency,
  ProductCategory,
  ProductIdentification,
  Brand,
  Model,
  InventoryItemKind,
  ProductType,
  Facility,
  Scope,
  Good,
  IUnitOfMeasure,
  SerialisedItemState,
  SerialisedItemAvailability,
  Ownership,
  Party,
  RequestState,
  QuoteState,
  SalesOrderState,
  SalesInvoiceState,
  ShipmentState,
  Product,
  SerialisedItem,
  SalesInvoiceType,
  PurchaseInvoiceType,
  PurchaseInvoiceState,
  Part,
  PurchaseOrderState,
  WorkEffortState,
  FixedAsset,
  Person,
  PositionType,
  RateType,
} from '@allors/domain/generated';
import { InternalOrganisationId, Filters } from '@allors/angular/base';

export function configure(metaPopulation: MetaPopulation, internalOrganisationId: InternalOrganisationId) {
  const m = metaPopulation as Meta;

  const currencySearch = new SearchFactory({
    objectType: m.Currency,
    roleTypes: [m.Currency.IsoCode],
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

  const scopeSearch = new SearchFactory({
    objectType: m.Scope,
    roleTypes: [m.Scope.Name],
  });

  const productSearch = new SearchFactory({
    objectType: m.Good,
    roleTypes: [m.Good.Name],
  });

  const partSearch = new SearchFactory({
    objectType: m.NonUnifiedPart,
    roleTypes: [m.NonUnifiedPart.Name, m.NonUnifiedPart.SearchString],
  });

  const productTypeSearch = new SearchFactory({
    objectType: m.ProductType,
    roleTypes: [m.ProductType.Name],
  });

  const productIdSearch = new SearchFactory({
    objectType: m.ProductIdentification,
    roleTypes: [m.ProductIdentification.Identification],
  });

  const uomSearch = new SearchFactory({
    objectType: m.IUnitOfMeasure,
    roleTypes: [m.IUnitOfMeasure.Name],
    predicates: [new Equals({ propertyType: m.IUnitOfMeasure.IsActive, value: true })],
  });

  const serialisedItemStateSearch = new SearchFactory({
    objectType: m.SerialisedItemState,
    roleTypes: [m.SerialisedItemState.Name],
    predicates: [new Equals({ propertyType: m.SerialisedItemState.IsActive, value: true })],
  });

  const serialisedItemAvailabilitySearch = new SearchFactory({
    objectType: m.SerialisedItemAvailability,
    roleTypes: [m.SerialisedItemAvailability.Name],
    predicates: [new Equals({ propertyType: m.SerialisedItemAvailability.IsActive, value: true })],
  });

  const ownershipSearch = new SearchFactory({
    objectType: m.Ownership,
    roleTypes: [m.Ownership.Name],
    predicates: [new Equals({ propertyType: m.Ownership.IsActive, value: true })],
  });

  const partySearch = new SearchFactory({
    objectType: m.Party,
    roleTypes: [m.Party.PartyName],
  });

  const requestStateSearch = new SearchFactory({
    objectType: m.RequestState,
    roleTypes: [m.RequestState.Name],
  });

  const quoteStateSearch = new SearchFactory({
    objectType: m.QuoteState,
    roleTypes: [m.QuoteState.Name],
  });

  const salesOrderstateSearch = new SearchFactory({
    objectType: m.SalesOrderState,
    roleTypes: [m.SalesOrderState.Name],
  });

  const salesOrderInvoiceStateSearch = new SearchFactory({
    objectType: m.SalesOrderInvoiceState,
    roleTypes: [m.SalesOrderInvoiceState.Name],
  });

  const salesOrderShipmentStateSearch = new SearchFactory({
    objectType: m.SalesOrderShipmentState,
    roleTypes: [m.SalesOrderShipmentState.Name],
  });

  const serialisedItemSearch = new SearchFactory({
    objectType: m.SerialisedItem,
    roleTypes: [m.SerialisedItem.ItemNumber],
  });

  const salesInvoiceTypeSearch = new SearchFactory({
    objectType: m.SalesInvoiceType,
    roleTypes: [m.SalesInvoiceType.Name],
  });

  const salesInvoiceStateSearch = new SearchFactory({
    objectType: m.SalesInvoiceState,
    roleTypes: [m.SalesInvoiceState.Name],
  });

  const purchaseInvoiceTypeSearch = new SearchFactory({
    objectType: m.PurchaseInvoiceType,
    roleTypes: [m.PurchaseInvoiceType.Name],
  });

  const purchaseInvoiceStateSearch = new SearchFactory({
    objectType: m.PurchaseInvoiceState,
    roleTypes: [m.PurchaseInvoiceState.Name],
  });

  const purchaseOrderStateSearch = new SearchFactory({
    objectType: m.PurchaseOrderState,
    roleTypes: [m.PurchaseOrderState.Name],
  });

  const shipmentStateSearch = new SearchFactory({
    objectType: m.ShipmentState,
    roleTypes: [m.ShipmentState.Name],
  });

  const workEffortStateSearch = new SearchFactory({
    objectType: m.WorkEffortState,
    roleTypes: [m.WorkEffortState.Name],
  });

  const personSearch = new SearchFactory({
    objectType: m.Person,
    roleTypes: [m.Person.PartyName, m.Person.UserName],
  });

  const fixedAssetSearch = new SearchFactory({
    objectType: m.FixedAsset,
    roleTypes: [m.FixedAsset.SearchString],
  });

  const positionTypeSearch = new SearchFactory({
    objectType: m.PositionType,
    roleTypes: [m.PositionType.Title],
  });

  const rateTypeSearch = new SearchFactory({
    objectType: m.RateType,
    roleTypes: [m.RateType.Name],
    predicates: [new Equals({ propertyType: m.RateType.IsActive, value: true })],
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
        search: () => Filters.internalOrganisationsFilter(m),
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
        search: () => Filters.internalOrganisationsFilter(m),
        initialValue: () => internalOrganisationId.value,
        display: (v: Organisation) => v && v.Name,
      },
      supplierFor: {
        search: () => Filters.internalOrganisationsFilter(m),
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
  m.RequestForQuote.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.Request.RequestState, parameter: 'state' }),
      new Equals({ propertyType: m.Request.Originator, parameter: 'from' }),
    ]), {
      active: { initialValue: true },
      state: { search: () => requestStateSearch, display: (v: RequestState) => v && v.Name },
      from: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
    }
  );
  m.RequestForQuote.sorter = new Sorter({
    number: m.Request.SortableRequestNumber,
    description: m.Request.Description,
    responseRequired: m.Request.RequiredResponseDate,
    lastModifiedDate: m.Request.LastModifiedDate,
  });

  m.ProductQuote.list = '/sales/productquotes';
  m.ProductQuote.overview = '/sales/productquote/:id';
  m.ProductQuote.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.ProductQuote.QuoteState, parameter: 'state' }),
      new Equals({ propertyType: m.ProductQuote.Receiver, parameter: 'to' }),
    ]), {
      active: { initialValue: true },
      state: { search: () => quoteStateSearch, display: (v: QuoteState) => v && v.Name },
      to: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
    }
  );
  m.ProductQuote.sorter = new Sorter({
    number: m.Quote.SortableQuoteNumber,
    description: m.Quote.Description,
    responseRequired: m.Quote.RequiredResponseDate,
    lastModifiedDate: m.Quote.LastModifiedDate,
  });

  m.SalesOrder.list = '/sales/salesorders';
  m.SalesOrder.overview = '/sales/salesorder/:id';
  m.SalesOrder.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.SalesOrder.OrderNumber, parameter: 'number' }),
      new Equals({ propertyType: m.SalesOrder.CustomerReference, parameter: 'customerReference' }),
      new Equals({ propertyType: m.SalesOrder.SalesOrderState, parameter: 'state' }),
      new Equals({ propertyType: m.SalesOrder.SalesOrderInvoiceState, parameter: 'invoiceState' }),
      new Equals({ propertyType: m.SalesOrder.SalesOrderShipmentState, parameter: 'shipmentState' }),
      new Equals({ propertyType: m.SalesOrder.ShipToCustomer, parameter: 'shipTo' }),
      new Equals({ propertyType: m.SalesOrder.BillToCustomer, parameter: 'billTo' }),
      new Equals({ propertyType: m.SalesOrder.ShipToEndCustomer, parameter: 'shipToEndCustomer' }),
      new Equals({ propertyType: m.SalesOrder.BillToEndCustomer, parameter: 'billToEndCustomer' }),
      new ContainedIn({
        propertyType: m.SalesOrder.SalesOrderItems,
        extent: new Extent({
          objectType: m.SalesOrderItem,
          predicate: new ContainedIn({
            propertyType: m.SalesOrderItem.Product,
            parameter: 'product',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.SalesOrder.SalesOrderItems,
        extent: new Extent({
          objectType: m.SalesOrderItem,
          predicate: new ContainedIn({
            propertyType: m.SalesOrderItem.SerialisedItem,
            parameter: 'serialisedItem',
          }),
        }),
      }),
    ]), {
      active: { initialValue: true },
      state: { search: () => salesOrderstateSearch, display: (v: SalesOrderState) => v && v.Name },
      invoiceState: { search: () => salesOrderInvoiceStateSearch, display: (v: SalesInvoiceState) => v && v.Name },
      shipmentState: { search: () => salesOrderShipmentStateSearch, display: (v: ShipmentState) => v && v.Name },
      shipTo: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      billTo: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      shipToEndCustomer: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      billToEndCustomer: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      product: { search: () => productSearch, display: (v: Product) => v && v.Name },
      serialisedItem: { search: () => serialisedItemSearch, display: (v: SerialisedItem) => v && v.ItemNumber },
    }
  );
  m.SalesOrder.sorter = new Sorter({
    number: m.SalesOrder.SortableOrderNumber,
    customerReference: m.SalesOrder.CustomerReference,
    lastModifiedDate: m.SalesOrder.LastModifiedDate,
  });

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
      supplier: {search: () => Filters.suppliersFilter(m, internalOrganisationId.value), display: (v: Organisation) => v && v.PartyName },
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
  m.Catalogue.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.Catalogue.Name, parameter: 'Name' }),
      new Equals({ propertyType: m.Catalogue.CatScope, parameter: 'Scope' }),
    ]), { Scope: { 
      search: () => scopeSearch, display: (v: Scope) => v && v.Name }
     }
  );
  m.Catalogue.sorter = new Sorter({
    name: m.Catalogue.Name,
    description: m.Catalogue.Description,
    scope: m.Scope.Name,
  });

  m.ProductCategory.list = '/products/productcategories';
  m.ProductCategory.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.ProductCategory.Name, parameter: 'name' }),
      new Equals({ propertyType: m.ProductCategory.CatScope, parameter: 'scope' }),
      new Contains({ propertyType: m.ProductCategory.Products, parameter: 'product' }),
    ]), {
      scope: { search: () => scopeSearch, display: (v: Scope) => v && v.Name },
      product: { search: () => productSearch, display: (v: Good) => v && v.Name },
    }
  );
  m.ProductCategory.sorter = new Sorter({
    name: m.Catalogue.Name,
    description: m.Catalogue.Description,
    scope: m.Scope.Name,
  });

  m.SerialisedItemCharacteristic.list = '/products/serialiseditemcharacteristics';
  m.SerialisedItemCharacteristic.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.SerialisedItemCharacteristicType.Name, parameter: 'name' }),
      new Equals({ propertyType: m.SerialisedItemCharacteristicType.IsActive, parameter: 'active' }),
      new Equals({ propertyType: m.SerialisedItemCharacteristicType.UnitOfMeasure, parameter: 'uom' }),
    ]), {
      active: { initialValue: true },
      uom: { search: () => uomSearch, display: (v: IUnitOfMeasure) => v && v.Name },
    }
  );
  m.SerialisedItemCharacteristic.sorter = new Sorter({
      name: m.SerialisedItemCharacteristicType.Name,
      uom: m.UnitOfMeasure.Name,
      active: m.SerialisedItemCharacteristicType.IsActive,
  });

  m.ProductType.list = '/products/producttypes';
  m.ProductType.filterDefinition = new FilterDefinition(
    new And([new Like({ roleType: m.ProductType.Name, parameter: 'name' })])
  );
  m.ProductType.sorter = new Sorter({
    name: m.ProductType.Name,
  });

  m.SerialisedItem.list = '/products/serialiseditems';
  m.SerialisedItem.overview = '/products/serialisedItem/:id';
  m.SerialisedItem.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.SerialisedItem.ItemNumber, parameter: 'id' }),
      new Like({ roleType: m.SerialisedItem.Name, parameter: 'name' }),
      new Like({ roleType: m.SerialisedItem.Keywords, parameter: 'keyword' }),
      new Equals({ propertyType: m.SerialisedItem.OnQuote, parameter: 'onQuote' }),
      new Equals({ propertyType: m.SerialisedItem.OnSalesOrder, parameter: 'onSalesOrder' }),
      new Equals({ propertyType: m.SerialisedItem.OnWorkEffort, parameter: 'onWorkEffort' }),
      new Equals({ propertyType: m.SerialisedItem.SerialisedItemAvailability, parameter: 'availability' }),
      new Equals({ propertyType: m.SerialisedItem.SerialisedItemState, parameter: 'state' }),
      new Equals({ propertyType: m.SerialisedItem.Ownership, parameter: 'ownership' }),
      new Equals({ propertyType: m.SerialisedItem.SuppliedBy, parameter: 'suppliedby' }),
      new Equals({ propertyType: m.SerialisedItem.RentedBy, parameter: 'rentedby' }),
      new Equals({ propertyType: m.SerialisedItem.OwnedBy, parameter: 'ownedby' }),
      new Like({ roleType: m.SerialisedItem.DisplayProductCategories, parameter: 'category' }),
      new ContainedIn({
        propertyType: m.SerialisedItem.PartWhereSerialisedItem,
        extent: new Extent({
          objectType: m.Part,
          predicate: new Equals({
            propertyType: m.Part.Brand,
            parameter: 'brand',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.SerialisedItem.PartWhereSerialisedItem,
        extent: new Extent({
          objectType: m.Part,
          predicate: new Equals({
            propertyType: m.Part.Model,
            parameter: 'model',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.SerialisedItem.PartWhereSerialisedItem,
        extent: new Extent({
          objectType: m.UnifiedGood,
          predicate: new ContainedIn({
            propertyType: m.UnifiedGood.ProductType,
            parameter: 'productType',
          }),
        }),
      }),
    ]), {
      active: { initialValue: true },
      onQuote: { initialValue: true },
      onSalesOrder: { initialValue: true },
      onWorkEffort: { initialValue: true },
      state: { search: () => serialisedItemStateSearch, display: (v: SerialisedItemState) => v && v.Name },
      availability: { search: () => serialisedItemAvailabilitySearch, display: (v: SerialisedItemAvailability) => v && v.Name },
      ownership: { search: () => ownershipSearch, display: (v: Ownership) => v && v.Name },
      suppliedby: { search: () => Filters.allSuppliersFilter(m), display: (v: Organisation) => v && v.Name },
      ownedby: { search: () => partySearch, display: (v: Party) => v && v.displayName },
      rentedby: { search: () => partySearch, display: (v: Party) => v && v.displayName },
      brand: { search: () => brandSearch, display: (v: Brand) => v && v.Name },
      model: { search: () => modelSearch, display: (v: Model) => v && v.Name },
      productType: { search: () => productTypeSearch, display: (v: ProductType) => v && v.Name },
    }
  );
  m.SerialisedItem.sorter = new Sorter({
    id: [m.SerialisedItem.ItemNumber],
    categories: [m.SerialisedItem.DisplayProductCategories],
    name: [m.SerialisedItem.Name],
    availability: [m.SerialisedItem.SerialisedItemAvailabilityName],
  });

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
  m.SalesInvoice.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.SalesInvoice.SalesInvoiceType, parameter: 'type' }),
      new Equals({ propertyType: m.SalesInvoice.InvoiceNumber, parameter: 'number' }),
      new Equals({ propertyType: m.SalesInvoice.CustomerReference, parameter: 'customerReference' }),
      new Equals({ propertyType: m.SalesInvoice.SalesInvoiceState, parameter: 'state' }),
      new Equals({ propertyType: m.SalesInvoice.ShipToCustomer, parameter: 'shipTo' }),
      new Equals({ propertyType: m.SalesInvoice.BillToCustomer, parameter: 'billTo' }),
      new Equals({ propertyType: m.SalesInvoice.ShipToEndCustomer, parameter: 'shipToEndCustomer' }),
      new Equals({ propertyType: m.SalesInvoice.BillToEndCustomer, parameter: 'billToEndCustomer' }),
      new Equals({ propertyType: m.SalesInvoice.IsRepeatingInvoice, parameter: 'repeating' }),
      new ContainedIn({
        propertyType: m.SalesInvoice.SalesInvoiceItems,
        extent: new Extent({
          objectType: m.SalesInvoiceItem,
          predicate: new ContainedIn({
            propertyType: m.SalesInvoiceItem.Product,
            parameter: 'product',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.SalesInvoice.SalesInvoiceItems,
        extent: new Extent({
          objectType: m.SalesInvoiceItem,
          predicate: new ContainedIn({
            propertyType: m.SalesInvoiceItem.SerialisedItem,
            parameter: 'serialisedItem',
          }),
        }),
      }),
    ]), {
      repeating: { initialValue: true },
      active: { initialValue: true },
      type: { search: () => salesInvoiceTypeSearch, display: (v: SalesInvoiceType) => v && v.Name },
      state: { search: () => salesInvoiceStateSearch, display: (v: SalesInvoiceState) => v && v.Name },
      shipTo: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      billTo: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      shipToEndCustomer: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      billToEndCustomer: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      product: { search: () => productSearch, display: (v: Product) => v && v.Name },
      serialisedItem: { search: () => serialisedItemSearch, display: (v: SerialisedItem) => v && v.ItemNumber },
    }
  );
  m.SalesInvoice.sorter = new Sorter({
    number: m.SalesInvoice.SortableInvoiceNumber,
    type: m.SalesInvoice.SalesInvoiceType,
    invoiceDate: m.SalesInvoice.InvoiceDate,
    description: m.SalesInvoice.Description,
    lastModifiedDate: m.SalesInvoice.LastModifiedDate,
  });

  m.PurchaseInvoice.list = '/purchasing/purchaseinvoices';
  m.PurchaseInvoice.overview = '/purchasing/purchaseinvoice/:id';
  m.PurchaseInvoice.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.PurchaseInvoice.InvoiceNumber, parameter: 'number' }),
      new Equals({ propertyType: m.PurchaseInvoice.PurchaseInvoiceState, parameter: 'state' }),
      new Equals({ propertyType: m.PurchaseInvoice.PurchaseInvoiceType, parameter: 'type' }),
      new Equals({ propertyType: m.PurchaseInvoice.BilledFrom, parameter: 'supplier' }),
      new ContainedIn({
        propertyType: m.PurchaseInvoice.PurchaseInvoiceItems,
        extent: new Extent({
          objectType: m.PurchaseInvoiceItem,
          predicate: new ContainedIn({
            propertyType: m.PurchaseInvoiceItem.Part,
            parameter: 'sparePart',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.PurchaseInvoice.PurchaseInvoiceItems,
        extent: new Extent({
          objectType: m.PurchaseInvoiceItem,
          predicate: new ContainedIn({
            propertyType: m.PurchaseInvoiceItem.SerialisedItem,
            parameter: 'serialisedItem',
          }),
        }),
      }),
    ]), {
      type: { search: () => purchaseInvoiceTypeSearch, display: (v: PurchaseInvoiceType) => v && v.Name },
      state: { search: () => purchaseInvoiceStateSearch, display: (v: PurchaseInvoiceState) => v && v.Name },
      supplier: { search: () => Filters.suppliersFilter(m, internalOrganisationId.value), display: (v: Party) => v && v.PartyName },
      sparePart: { search: () => partSearch, display: (v: Part) => v && v.Name },
      serialisedItem: { search: () => serialisedItemSearch, display: (v: SerialisedItem) => v && v.ItemNumber },
    }
  );
  m.PurchaseInvoice.sorter = new Sorter({
    number: m.PurchaseInvoice.SortableInvoiceNumber,
    type: m.PurchaseInvoice.PurchaseInvoiceType,
    reference: m.PurchaseInvoice.CustomerReference,
    dueDate: m.PurchaseInvoice.DueDate,
    totalExVat: m.PurchaseInvoice.TotalExVat,
    totalIncVat: m.PurchaseInvoice.TotalIncVat,
    lastModifiedDate: m.PurchaseInvoice.LastModifiedDate,
  });

  m.PurchaseOrder.list = '/purchasing/purchaseorders';
  m.PurchaseOrder.overview = '/purchasing/purchaseorder/:id';
  m.PurchaseOrder.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.PurchaseOrder.OrderNumber, parameter: 'number' }),
      new Equals({ propertyType: m.PurchaseOrder.CustomerReference, parameter: 'customerReference' }),
      new Equals({ propertyType: m.PurchaseOrder.PurchaseOrderState, parameter: 'state' }),
      new Equals({ propertyType: m.PurchaseOrder.TakenViaSupplier, parameter: 'supplier' }),
      new ContainedIn({
        propertyType: m.PurchaseOrder.PurchaseOrderItems,
        extent: new Extent({
          objectType: m.PurchaseOrderItem,
          predicate: new ContainedIn({
            propertyType: m.PurchaseOrderItem.Part,
            parameter: 'sparePart',
          }),
        }),
      }),
      new ContainedIn({
        propertyType: m.PurchaseOrder.PurchaseOrderItems,
        extent: new Extent({
          objectType: m.PurchaseOrderItem,
          predicate: new ContainedIn({
            propertyType: m.PurchaseOrderItem.SerialisedItem,
            parameter: 'serialisedItem',
          }),
        }),
      }),
    ]), {
      active: { initialValue: true },
      state: { search: () => purchaseOrderStateSearch, display: (v: PurchaseOrderState) => v && v.Name },
      supplier: { search: () => Filters.suppliersFilter(m, internalOrganisationId.value), display: (v: Party) => v && v.PartyName },
      sparePart: { search: () => partSearch, display: (v: Part) => v && v.Name },
      serialisedItem: { search: () => serialisedItemSearch, display: (v: SerialisedItem) => v && v.ItemNumber },
    }
  );
  m.PurchaseOrder.sorter = new Sorter({
    number: m.PurchaseOrder.SortableOrderNumber,
    customerReference: m.PurchaseOrder.CustomerReference,
    totalExVat: m.PurchaseOrder.TotalExVat,
    totalIncVat: m.PurchaseOrder.TotalIncVat,
    lastModifiedDate: m.PurchaseOrder.LastModifiedDate,
  });

  m.Shipment.list = '/shipment/shipments';
  m.Shipment.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.Shipment.ShipmentNumber, parameter: 'number' }),
      new Equals({ propertyType: m.Shipment.ShipmentState, parameter: 'state' }),
      new Equals({ propertyType: m.Shipment.ShipFromParty, parameter: 'shipFrom' }),
      new Equals({ propertyType: m.Shipment.ShipToParty, parameter: 'shipTo' }),
      new ContainedIn({
        propertyType: m.Shipment.ShipmentItems,
        extent: new Extent({
          objectType: m.ShipmentItem,
          predicate: new ContainedIn({
            propertyType: m.ShipmentItem.Part,
            parameter: 'part',
          }),
        }),
      }),
    ]), {
      active: { initialValue: true },
      state: { search: () => shipmentStateSearch, display: (v: ShipmentState) => v && v.Name },
      shipFrom: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      shipTo: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
    }
  );
  m.Shipment.sorter = new Sorter({
    number: m.Shipment.SortableShipmentNumber,
    from: m.Shipment.ShipFromParty,
    to: m.Shipment.ShipToParty,
    lastModifiedDate: m.Shipment.LastModifiedDate,
  });

  m.CustomerShipment.overview = '/shipment/customershipment/:id';
  m.PurchaseShipment.overview = '/shipment/purchaseshipment/:id';

  m.Carrier.list = '/shipment/carriers';
  m.Carrier.filterDefinition = new FilterDefinition(
    new And([
      new Like({ roleType: m.Carrier.Name, parameter: 'name' }),
    ])
  );
  m.Carrier.sorter = new Sorter({
    name: m.Carrier.Name,
  });

  m.WorkEffort.list = '/workefforts/workefforts';
  m.WorkEffort.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.WorkEffort.WorkEffortState, parameter: 'state' }),
      new Equals({ propertyType: m.WorkEffort.Customer, parameter: 'customer' }),
      new Equals({ propertyType: m.WorkEffort.ExecutedBy, parameter: 'ExecutedBy' }),
      new Like({ roleType: m.WorkEffort.WorkEffortNumber, parameter: 'Number' }),
      new Like({ roleType: m.WorkEffort.Name, parameter: 'Name' }),
      new Like({ roleType: m.WorkEffort.Description, parameter: 'Description' }),
      new ContainedIn({
        propertyType: m.WorkEffort.WorkEffortPartyAssignmentsWhereAssignment,
        extent: new Extent({
          objectType: m.WorkEffortPartyAssignment,
          predicate: new Equals({ propertyType: m.WorkEffortPartyAssignment.Party, parameter: 'worker' }),
        }),
      }),
      new ContainedIn({
        propertyType: m.WorkEffort.WorkEffortFixedAssetAssignmentsWhereAssignment,
        extent: new Extent({
          objectType: m.WorkEffortFixedAssetAssignment,
          predicate: new Equals({ propertyType: m.WorkEffortFixedAssetAssignment.FixedAsset, parameter: 'equipment' }),
        }),
      }),
    ]), {
      state: { search: () => workEffortStateSearch, display: (v: WorkEffortState) => v && v.Name },
      customer: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      ExecutedBy: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      worker: { search: () => personSearch, display: (v: Person) => v && v.displayName },
      equipment: { search: () => fixedAssetSearch, display: (v: FixedAsset) => v && v.displayName },
    }
  );
  m.WorkEffort.sorter = new Sorter({
    number: [m.WorkEffort.SortableWorkEffortNumber],
    name: [m.WorkEffort.Name],
    description: [m.WorkEffort.Description],
    lastModifiedDate: m.Person.LastModifiedDate,
  });

  m.WorkTask.overview = '/workefforts/worktask/:id';

  m.PositionType.list = '/humanresource/positiontypes';
  m.PositionType.filterDefinition = new FilterDefinition(
    new And([new Like({ roleType: m.PositionType.Title, parameter: 'title' })])
  );
  m.PositionType.sorter = new Sorter({
    title: m.PositionType.Title,
  });

  m.PositionTypeRate.list = '/humanresource/positiontyperates';
  m.PositionTypeRate.filterDefinition = new FilterDefinition(
    new And([
      new Contains({ propertyType: m.PositionTypeRate.PositionTypesWherePositionTypeRate, parameter: 'positionType' }),
      new Equals({ propertyType: m.PositionTypeRate.RateType, parameter: 'rateType' }),
    ]), {
      active: { initialValue: true },
      positionType: { search: () => positionTypeSearch, display: (v: PositionType) => v && v.Title },
      rateType: { search: () => rateTypeSearch, display: (v: RateType) => v && v.Name },
    }
  );
  m.PositionTypeRate.sorter = new Sorter({
    rate: m.PositionTypeRate.Rate,
    from: m.PositionTypeRate.FromDate,
    through: m.PositionTypeRate.ThroughDate,
  });

  m.TaskAssignment.list = '/workflow/taskassignments';

  m.ExchangeRate.list = '/accounting/exchangerates';
  m.ExchangeRate.filterDefinition = new FilterDefinition(
    new And([
      new Equals({ propertyType: m.ExchangeRate.FromCurrency, parameter: 'fromCurrency' }),
      new Equals({ propertyType: m.ExchangeRate.ToCurrency, parameter: 'toCurrency' }),
    ]),
    {
      fromCurrency: { search: () => currencySearch, display: (v: Currency) => v && v.IsoCode },
      toCurrency: { search: () => currencySearch, display: (v: Currency) => v && v.IsoCode },
    }
  );
  m.ExchangeRate.sorter = new Sorter({
    validFrom: m.ExchangeRate.ValidFrom,
    from: m.ExchangeRate.FromCurrency,
    to: m.ExchangeRate.ToCurrency,
  });
}
