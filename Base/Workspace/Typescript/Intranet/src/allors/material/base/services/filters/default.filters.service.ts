import { Injectable } from '@angular/core';
import { SearchFactory, InternalOrganisationId, MetaService } from '../../../../angular';
import { And, ContainedIn, Equals, Filter, Or } from '../../../../framework';
import { Meta, TreeFactory } from '../../../../meta';

import { FiltersService } from './filters.service';

@Injectable()
export class DefaultFiltersService extends FiltersService {

  private m: Meta;
  private tree: TreeFactory;

  constructor(
    private metaService: MetaService,
    private internalOrganisationId: InternalOrganisationId,
  ) {
    super();

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
        predicate.operands.push(new ContainedIn({
          propertyType: this.m.UnifiedGood.InventoryItemKind,
          extent: new Filter({
            objectType: this.m.InventoryItemKind,
            predicate: new Equals({ propertyType: this.m.InventoryItemKind.UniqueId, value: '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae' }),
          })
        }));
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
      include: this.tree.UnifiedGood({SerialisedItems: {}})
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
        predicate.operands.push(new ContainedIn({
          propertyType: this.m.Party.CustomerRelationshipsWhereCustomer,
          extent: new Filter({
            objectType: this.m.CustomerRelationship,
            predicate: new Equals({ propertyType: this.m.CustomerRelationship.InternalOrganisation, object: this.internalOrganisationId.value }),
          })
        }));
      },
    });
  }

  get suppliersFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(new ContainedIn({
          propertyType: this.m.Organisation.SupplierRelationshipsWhereSupplier,
          extent: new Filter({
            objectType: this.m.SupplierRelationship,
            predicate: new Equals({ propertyType: this.m.SupplierRelationship.InternalOrganisation, object: this.internalOrganisationId.value }),
          })
        }));
      },
    });
  }

  get allSuppliersFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(new ContainedIn({
          propertyType: this.m.Organisation.SupplierRelationshipsWhereSupplier,
          extent: new Filter({
            objectType: this.m.SupplierRelationship,
          })
        }));
      },
    });
  }

  get subContractorsFilter() {
    return new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.PartyName],
      post: (predicate: And) => {
        predicate.operands.push(new ContainedIn({
          propertyType: this.m.Organisation.SubContractorRelationshipsWhereSubContractor,
          extent: new Filter({
            objectType: this.m.SubContractorRelationship,
            predicate: new Equals({ propertyType: this.m.SubContractorRelationship.Contractor, object: this.internalOrganisationId.value }),
          })
        }));
      },
    });
  }

  get employeeFilter() {
    return new SearchFactory({
      objectType: this.m.Person,
      roleTypes: [this.m.Person.PartyName, this.m.Person.UserName],
      post: (predicate: And) => {
        predicate.operands.push(new ContainedIn({
          propertyType: this.m.Person.EmploymentsWhereEmployee,
          extent: new Filter({
            objectType: this.m.Employment,
            predicate: new Equals({ propertyType: this.m.Employment.Employer, object: this.internalOrganisationId.value }),
          })
        }));
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
