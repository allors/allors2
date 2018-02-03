import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Organisation } from "../../../domain";
import { StateService } from "./StateService";

@Injectable()
export class DefaultStateService extends StateService {

    private internalOrganisationSubject;

    constructor() {
        super();
        this.internalOrganisationSubject = new BehaviorSubject(null);
        this.internalOrganisation$ = this.internalOrganisationSubject;
    }

    public selectInternalOrginsation(internalOrganisation: Organisation) {
        this.internalOrganisationSubject.next(internalOrganisation.id);
    }
}
