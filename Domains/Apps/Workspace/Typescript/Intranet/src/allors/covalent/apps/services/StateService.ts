import { Observable } from "rxjs/Observable";
import { Organisation } from "../../../domain";

export abstract class StateService {
    public singletonId: string;

    public internalOrganisationId: string;
    public internalOrganisationId$: Observable<string>;
}
