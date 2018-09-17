import { Observable } from 'rxjs';
import { SearchFactory } from '../../../angular';
import { Organisation } from '../../../domain';

export abstract class StateService {
    public singletonId: string;

    public internalOrganisationId: string;
    public internalOrganisationId$: Observable<string>;

    public goodsFilter: SearchFactory;
    public customersFilter: SearchFactory;
    public organisationsFilter: SearchFactory;
}
