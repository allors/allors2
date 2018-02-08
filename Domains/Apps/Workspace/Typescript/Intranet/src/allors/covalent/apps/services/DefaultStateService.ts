import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Organisation } from "../../../domain";
import { StateService } from "./StateService";

@Injectable()
export class DefaultStateService extends StateService {
    private static readonly internalOrganisationIdKey = "StateService$InternalOrganisationId";
    private static readonly singletonIdKey = "StateService$SingletonId";

    private internalOrganisationIdSubject: BehaviorSubject<string>;

    constructor() {
        super();

        const sessionInternalOrganisationId = sessionStorage.getItem(DefaultStateService.internalOrganisationIdKey);
        this.internalOrganisationIdSubject = new BehaviorSubject(sessionInternalOrganisationId);
        this.internalOrganisationId$ = this.internalOrganisationIdSubject;
    }

    public get singletonId(): string {
        const key = DefaultStateService.singletonIdKey;
        return sessionStorage.getItem(key);
    }

    public set singletonId(value: string) {
        const key = DefaultStateService.singletonIdKey;
        sessionStorage.setItem(key, value);
    }

    public get internalOrganisationId(): string {
        return this.internalOrganisationIdSubject.value;
    }

    public set internalOrganisationId(value: string) {
        sessionStorage.setItem(DefaultStateService.internalOrganisationIdKey, value);
        this.internalOrganisationIdSubject.next(value);
    }
}
