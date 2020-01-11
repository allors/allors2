import { Observable } from 'rxjs';
import { SearchFactory } from '../../../../angular';

export abstract class FiltersService {
    goodsFilter: SearchFactory;
    partsFilter: SearchFactory;
    serialisedItemsFilter: SearchFactory;
    customersFilter: SearchFactory;
    suppliersFilter: SearchFactory;
    employeeFilter: SearchFactory;
    organisationsFilter: SearchFactory;
    peopleFilter: SearchFactory;
    partiesFilter: SearchFactory;
}
