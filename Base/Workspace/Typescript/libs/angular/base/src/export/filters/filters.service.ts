import { Injectable } from '@angular/core';
import { Meta, TreeFactory } from '@allors/meta/generated';
import { MetaService } from '@allors/angular/services/core';
import { And, ContainedIn, Extent, Equals } from '@allors/data/system';

import { InternalOrganisationId } from '../state/InternalOrganisationId';
import { SearchFactory } from '@allors/angular/core';

@Injectable({
  providedIn: 'root',
})
export class FiltersService {
  protected m: Meta;
  protected tree: TreeFactory;

  constructor(protected metaService: MetaService, protected internalOrganisationId: InternalOrganisationId) {
    this.m = this.metaService.m;
    this.tree = this.metaService.tree;
  }

  get goodsFilter() {
    return new SearchFactory({
      objectType: this.m.Good,
      roleTypes: [this.m.Good.Name, this.m.Good.SearchString],
    });
  }

  get serialisedgoodsFilter() {
    return new SearchFactory({
      objectType: this.m.UnifiedGood,
      roleTypes: [this.m.UnifiedGood.Name, this.m.UnifiedGood.SearchString],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: this.m.UnifiedGood.InventoryItemKind,
            extent: new Extent({
              objectType: this.m.InventoryItemKind,
              predicate: new Equals({ propertyType: this.m.InventoryItemKind.UniqueId, value: '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae' }),
            }),
          })
        );
      },
    });
  }

  get partsFilter() {
    return new SearchFactory({
      objectType: this.m.Part,
      roleTypes: [this.m.Part.Name, this.m.Part.SearchString],
    });
  }

  get nonUnifiedPartsFilter() {
    return new SearchFactory({
      objectType: this.m.NonUnifiedPart,
      roleTypes: [this.m.NonUnifiedPart.Name, this.m.NonUnifiedPart.SearchString],
    });
  }

  get unifiedGoodsFilter() {
    return new SearchFactory({
      objectType: this.m.UnifiedGood,
      roleTypes: [this.m.UnifiedGood.Name, this.m.UnifiedGood.SearchString],
      include: this.tree.UnifiedGood({ SerialisedItems: {}, PartWeightedAverage: {} }),
    });
  }

  get serialisedItemsFilter() {
    return new SearchFactory({
      objectType: this.m.SerialisedItem,
      roleTypes: [this.m.SerialisedItem.Name, this.m.SerialisedItem.SearchString],
    });
  }

  get customersFilter() {
    return new SearchFactory({
      objectType: this.m.Party,
      roleTypes: [this.m.Party.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: this.m.Party.CustomerRelationshipsWhereCustomer,
            extent: new Extent({
              objectType: this.m.CustomerRelationship,
              predicate: new Equals({
                propertyType: this.m.CustomerRelationship.InternalOrganisation,
                object: this.internalOrganisationId.value,
              }),
            }),
          })
        );
      },
    });
  }

  get suppliersFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: this.m.Organisation.SupplierRelationshipsWhereSupplier,
            extent: new Extent({
              objectType: this.m.SupplierRelationship,
              predicate: new Equals({
                propertyType: this.m.SupplierRelationship.InternalOrganisation,
                object: this.internalOrganisationId.value,
              }),
            }),
          })
        );
      },
    });
  }

  get allSuppliersFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: this.m.Organisation.SupplierRelationshipsWhereSupplier,
            extent: new Extent({
              objectType: this.m.SupplierRelationship,
            }),
          })
        );
      },
    });
  }

  get subContractorsFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: this.m.Organisation.SubContractorRelationshipsWhereSubContractor,
            extent: new Extent({
              objectType: this.m.SubContractorRelationship,
              predicate: new Equals({
                propertyType: this.m.SubContractorRelationship.Contractor,
                object: this.internalOrganisationId.value,
              }),
            }),
          })
        );
      },
    });
  }

  get employeeFilter() {
    return new SearchFactory({
      objectType: this.m.Person,
      roleTypes: [this.m.Person.PartyName, this.m.Person.UserName],
      post: (predicate: And) => {
        predicate.operands.push(
          new ContainedIn({
            propertyType: this.m.Person.EmploymentsWhereEmployee,
            extent: new Extent({
              objectType: this.m.Employment,
              predicate: new Equals({ propertyType: this.m.Employment.Employer, object: this.internalOrganisationId.value }),
            }),
          })
        );
      },
    });
  }

  get organisationsFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
    });
  }

  get internalOrganisationsFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(new Equals({ propertyType: this.m.Organisation.IsInternalOrganisation, value: true }));
      },
    });
  }

  get peopleFilter() {
    return new SearchFactory({
      objectType: this.m.Person,
      roleTypes: [this.m.Person.PartyName],
    });
  }

  get partiesFilter() {
    return new SearchFactory({
      objectType: this.m.Party,
      roleTypes: [this.m.Party.PartyName],
    });
  }

  get workEffortsFilter() {
    return new SearchFactory({
      objectType: this.m.WorkEffort,
      roleTypes: [this.m.WorkEffort.Name],
    });
  }
}
