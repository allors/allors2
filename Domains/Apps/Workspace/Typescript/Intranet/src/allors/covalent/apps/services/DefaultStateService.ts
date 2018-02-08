import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Organisation } from "../../../domain";
import { StateService } from "./StateService";

@Injectable()
export class DefaultStateService extends StateService {
    private static readonly internalOrganisationKey = "StateServiceInternalOrganisation";
    private static readonly singletonKey = "StateServiceSingleton";

    private internalOrganisationSubject;

    constructor() {
        super();

        const key = DefaultStateService.internalOrganisationKey;
        const id = sessionStorage.getItem(key);
        this.internalOrganisationSubject = new BehaviorSubject(id);
        this.internalOrganisation$ = this.internalOrganisationSubject;
    }

    public get singleton(): string {
        const key = DefaultStateService.singletonKey;
        return sessionStorage.getItem(key);
    }

    public set singleton(value: string) {
        const key = DefaultStateService.singletonKey;
        sessionStorage.setItem(key, value);
    }

    public selectInternalOrganisation(internalOrganisation: Organisation) {

        const key = DefaultStateService.internalOrganisationKey;
        const id = internalOrganisation.id;
        sessionStorage.setItem(key, id);
        this.internalOrganisationSubject.next(id);
    }
}
