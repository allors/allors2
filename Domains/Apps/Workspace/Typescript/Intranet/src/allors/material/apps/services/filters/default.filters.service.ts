import { Injectable } from '@angular/core';
import { SearchFactory, WorkspaceService, InternalOrganisationId } from '../../../../angular';
import { And, ContainedIn, Equals, Filter } from '../../../../framework';
import { Meta } from '../../../../meta';

import { FiltersService } from './filters.service';

@Injectable()
export class DefaultFiltersService extends FiltersService {

  private m: Meta;

  constructor(
    private workspaceService: WorkspaceService,
    private internalOrganisationId: InternalOrganisationId,
  ) {
    super();

    this.m = this.workspaceService.metaPopulation as Meta;
  }

  get goodsFilter() {
    return new SearchFactory({
      objectType: this.m.Good,
      roleTypes: [this.m.Good.Name],
    })
  }

  get partsFilter() {
    return new SearchFactory({
      objectType: this.m.Part,
      roleTypes: [this.m.Part.Name],
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

  get employeeFilter() {
    return new SearchFactory({
      objectType: this.m.Person,
      roleTypes: [this.m.Person.PartyName],
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

  get partiesFilter() {
    return new SearchFactory({
      objectType: this.m.Party,
      roleTypes: [this.m.Party.PartyName],
    });
  }
}
