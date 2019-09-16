import { Observable } from 'rxjs';
import { SearchFactory } from '../../../../angular';

export abstract class FiltersService {
    goodsFilter: SearchFactory;
    partsFilter: SearchFactory;
    customersFilter: SearchFactory;
    suppliersFilter: SearchFactory;
    employeeFilter: SearchFactory;
    organisationsFilter: SearchFactory;
    partiesFilter: SearchFactory;
}