import { Observable } from "rxjs/Observable";
import { Organisation } from "../../../domain";

export abstract class StateService {
    public internalOrganisation$: Observable<string>;

    public abstract selectInternalOrganisation(internalOrganisation: Organisation);
}
