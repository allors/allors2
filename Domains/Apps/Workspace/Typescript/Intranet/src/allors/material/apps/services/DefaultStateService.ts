import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SearchFactory, WorkspaceService } from '../../../angular';
import { And, ContainedIn, Equals, Filter } from '../../../framework';
import { MetaDomain } from '../../../meta';
import { StateService } from './StateService';

@Injectable()
export class DefaultStateService extends StateService {
    private static readonly internalOrganisationsKey = 'StateService$InternalOrganisations';
    private static readonly internalOrganisationIdKey = 'StateService$InternalOrganisationId';
    private static readonly singletonIdKey = 'StateService$SingletonId';

    private internalOrganisationIdSubject: BehaviorSubject<string>;

    constructor(private workspaceService: WorkspaceService) {
        super();

        const sessionInternalOrganisationId = sessionStorage.getItem(DefaultStateService.internalOrganisationIdKey);
        this.internalOrganisationIdSubject = new BehaviorSubject(sessionInternalOrganisationId);
        this.internalOrganisationId$ = this.internalOrganisationIdSubject;

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        this.goodsFilter = new SearchFactory({
            objectType: m.Good,
            roleTypes: [m.Good.Name],
            post: (predicate: And) => {
                predicate.operands.push(new ContainedIn({
                    propertyType: m.Product.VendorProductsWhereProduct,
                    extent: new Filter({
                        objectType: m.VendorProduct,
                        predicate: new Equals({ propertyType: m.VendorProduct.InternalOrganisation, value: this.internalOrganisationId }),
                    })
                }));
            },
        });

        this.customersFilter = new SearchFactory({
            objectType: m.Party,
            roleTypes: [m.Party.PartyName],
            post: (predicate: And) => {
                predicate.operands.push(new ContainedIn({
                    propertyType: m.Party.CustomerRelationshipsWhereCustomer, 
                    extent: new Filter({
                        objectType: m.CustomerRelationship,
                        predicate: new Equals({ propertyType: m.CustomerRelationship.InternalOrganisation, value: this.internalOrganisationId }),
                    })
                }));
            },
        });

        this.organisationsFilter = new SearchFactory({
            objectType: m.Organisation,
            roleTypes: [m.Organisation.PartyName],
        });
    }

    public get singletonId(): string {
        const key = DefaultStateService.singletonIdKey;
        return sessionStorage.getItem(key);
    }

    public set singletonId(value: string) {
        const key = DefaultStateService.singletonIdKey;
        sessionStorage.setItem(key, value);
    }

    public get internalOrganisationId(): string {
        return this.internalOrganisationIdSubject.value;
    }

    public set internalOrganisationId(value: string) {
        sessionStorage.setItem(DefaultStateService.internalOrganisationIdKey, value);
        this.internalOrganisationIdSubject.next(value);
    }
}
