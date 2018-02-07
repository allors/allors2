import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Organisation } from "../../../domain";
import { StateService } from "./StateService";

@Injectable()
export class DefaultStateService extends StateService {
    private static readonly internalOrganisationKey = "StateServiceInternalOrganisation";

    private internalOrganisationSubject;

    constructor() {
        super();

        const key = DefaultStateService.internalOrganisationKey;
        const id = sessionStorage.getItem(key);
        this.internalOrganisationSubject = new BehaviorSubject(id);
        this.internalOrganisation$ = this.internalOrganisationSubject;
    }

    public selectInternalOrganisation(internalOrganisation: Organisation) {

        const key = DefaultStateService.internalOrganisationKey;
        const id = internalOrganisation.id;
        sessionStorage.setItem(key, id);
        this.internalOrganisationSubject.next(id);
    }
}
