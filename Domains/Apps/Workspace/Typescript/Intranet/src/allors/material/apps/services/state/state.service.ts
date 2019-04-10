import { Observable } from 'rxjs';
import { SearchFactory } from '../../../../angular';

export abstract class StateService {
    singletonId: string;
    userId: string;

    internalOrganisationId: string;
    internalOrganisationId$: Observable<string>;

    goodsFilter: SearchFactory;
    partsFilter: SearchFactory;
    customersFilter: SearchFactory;
    suppliersFilter: SearchFactory;
    employeeFilter: SearchFactory;
    organisationsFilter: SearchFactory;
    partiesFilter: SearchFactory;
}
