import { Observable } from "rxjs/Observable";
import { FilterFactory } from "../../../angular";
import { Organisation } from "../../../domain";

export abstract class StateService {
    public singletonId: string;

    public internalOrganisationId: string;
    public internalOrganisationId$: Observable<string>;

    public goodsFilter: FilterFactory;
    public customersFilter: FilterFactory;
    public organisationsFilter: FilterFactory;
}
