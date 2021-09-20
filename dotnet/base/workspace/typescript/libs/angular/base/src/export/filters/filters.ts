import { Meta, TreeFactory } from '@allors/meta/generated';
import { And, ContainedIn, Extent, Equals, Tree } from '@allors/data/system';
import { SearchFactory } from '@allors/angular/core';

export class Filters {
   static goodsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.Good,
      roleTypes: [m.Good.Name, m.Good.SearchString],
    });
  }

  static serialisedgoodsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.UnifiedGood,
      roleTypes: [m.UnifiedGood.Name, m.UnifiedGood.SearchString],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: m.UnifiedGood.InventoryItemKind,
            extent: new Extent({
              objectType: m.InventoryItemKind,
              predicate: new Equals({ propertyType: m.InventoryItemKind.UniqueId, value: '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae' }),
            }),
          })
        );
      },
    });
  }

  static partsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.Part,
      roleTypes: [m.Part.Name, m.Part.SearchString],
    });
  }

  static nonUnifiedPartsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.NonUnifiedPart,
      roleTypes: [m.NonUnifiedPart.Name, m.NonUnifiedPart.SearchString],
    });
  }

  static unifiedGoodsFilter(m: Meta, treeFactory: TreeFactory) {
    return new SearchFactory({
      objectType: m.UnifiedGood,
      roleTypes: [m.UnifiedGood.Name, m.UnifiedGood.SearchString],
      include: treeFactory.UnifiedGood({ SerialisedItems: {}, PartWeightedAverage: {} }),
    });
  }

  static serialisedItemsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.SerialisedItem,
      roleTypes: [m.SerialisedItem.Name, m.SerialisedItem.SearchString],
    });
  }

  static customersFilter(m: Meta, internalOrganisationId: string) {
    return new SearchFactory({
      objectType: m.Party,
      roleTypes: [m.Party.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: m.Party.CustomerRelationshipsWhereCustomer,
            extent: new Extent({
              objectType: m.CustomerRelationship,
              predicate: new Equals({
                propertyType: m.CustomerRelationship.InternalOrganisation,
                object: internalOrganisationId,
              }),
            }),
          })
        );
      },
    });
  }

  static suppliersFilter(m: Meta, internalOrganisationId: string) {
    return new SearchFactory({
      objectType: m.Organisation,
      roleTypes: [m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: m.Organisation.SupplierRelationshipsWhereSupplier,
            extent: new Extent({
              objectType: m.SupplierRelationship,
              predicate: new Equals({
                propertyType: m.SupplierRelationship.InternalOrganisation,
                object: internalOrganisationId,
              }),
            }),
          })
        );
      },
    });
  }

  static allSuppliersFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.Organisation,
      roleTypes: [m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: m.Organisation.SupplierRelationshipsWhereSupplier,
            extent: new Extent({
              objectType: m.SupplierRelationship,
            }),
          })
        );
      },
    });
  }

  static subContractorsFilter(m: Meta, internalOrganisationId: string) {
    return new SearchFactory({
      objectType: m.Organisation,
      roleTypes: [m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: m.Organisation.SubContractorRelationshipsWhereSubContractor,
            extent: new Extent({
              objectType: m.SubContractorRelationship,
              predicate: new Equals({
                propertyType: m.SubContractorRelationship.Contractor,
                object: internalOrganisationId,
              }),
            }),
          })
        );
      },
    });
  }

  static employeeFilter(m: Meta, internalOrganisationId: string) {
    return new SearchFactory({
      objectType: m.Person,
      roleTypes: [m.Person.PartyName, m.Person.UserName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: m.Person.EmploymentsWhereEmployee,
            extent: new Extent({
              objectType: m.Employment,
              predicate: new Equals({ propertyType: m.Employment.Employer, object: internalOrganisationId }),
            }),
          })
        );
      },
    });
  }

  static organisationsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.Organisation,
      roleTypes: [m.Organisation.PartyName],
    });
  }

  static internalOrganisationsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.Organisation,
      roleTypes: [m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }));
      },
    });
  }

  static peopleFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.Person,
      roleTypes: [m.Person.PartyName],
    });
  }

  static partiesFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.Party,
      roleTypes: [m.Party.PartyName],
    });
  }

  static workEffortsFilter(m: Meta) {
    return new SearchFactory({
      objectType: m.WorkEffort,
      roleTypes: [m.WorkEffort.Name],
    });
  }
}
