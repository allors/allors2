import { Observable } from 'rxjs';
import { SearchFactory } from '../../../../angular';

export abstract class StateService {
    public singletonId: string;

    public internalOrganisationId: string;
    public internalOrganisationId$: Observable<string>;

    public goodsFilter: SearchFactory;
    public partsFilter: SearchFactory;
    public customersFilter: SearchFactory;
    public suppliersFilter: SearchFactory;
    public employeeFilter: SearchFactory;
    public organisationsFilter: SearchFactory;
    public partiesFilter: SearchFactory;
}
