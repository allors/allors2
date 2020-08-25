import { MetaPopulation } from '@allors/meta/system';
import { Meta } from '@allors/meta/generated';

import '@allors/meta/core';
import { FilterDefinition, SearchFactory } from '@allors/angular/core';
import { And, Like, ContainedIn, Extent, Equals } from '@allors/data/system';
import { Sorter } from '@allors/angular/material/core';
import { Organisation, Country } from '@allors/domain/generated';
import { InternalOrganisationId } from '@allors/angular/base';

export function configure(metaPopulation: MetaPopulation, internalOrganisationId: InternalOrganisationId) {
  const m = metaPopulation as Meta;

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
    ]), {
      customerAt: {
        search: internalOrganisationSearch,
        display: (v: Organisation) => v && v.Name,
      },
      country: { search: countrySearch, display: (v: Country) => v && v.Name },
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
    ]),  {
      customerAt: {
        search: internalOrganisationSearch,
        initialValue: () => internalOrganisationId.value,
        display: (v: Organisation) => v && v.Name,
      },
      supplierFor: {
        search: internalOrganisationSearch,
        initialValue: () => internalOrganisationId.value,
        display: (v: Organisation) => v && v.Name,
      },
      country: {
        search: countrySearch,
        display: (v: Country) => v && v.Name,
      },
    }
  );
  m.Organisation.sorter = new Sorter({
    name: m.Organisation.Name,
    lastModifiedDate: m.Organisation.LastModifiedDate,
  });

  m.CommunicationEvent.list = '/contacts/communicationevents';

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
  m.NonUnifiedGood.overview = '/products/nonunifiedgood/:id';
  m.Part.list = '/products/parts';
  m.NonUnifiedPart.overview = '/products/nonunifiedpart/:id';
  m.Catalogue.list = '/products/catalogues';
  m.ProductCategory.list = '/products/productcategories';
  m.SerialisedItemCharacteristic.list = '/products/serialiseditemcharacteristics';
  m.ProductType.list = '/products/producttypes';
  m.SerialisedItem.list = '/products/serialiseditems';
  m.SerialisedItem.overview = '/products/serialisedItem/:id';
  m.UnifiedGood.list = '/products/unifiedgoods';
  m.UnifiedGood.overview = '/products/unifiedgood/:id';

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
