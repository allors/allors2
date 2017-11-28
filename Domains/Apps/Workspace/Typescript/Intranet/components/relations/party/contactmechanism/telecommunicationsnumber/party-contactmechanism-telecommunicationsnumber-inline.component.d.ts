import { EventEmitter, OnInit } from "@angular/core";
import { MetaDomain, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber } from "@allors/workspace";
import { Scope, WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismTelecommunicationsNumberInlineComponent implements OnInit {
    private workspaceService;
    private errorService;
    saved: EventEmitter<PartyContactMechanism>;
    cancelled: EventEmitter<any>;
    scope: Scope;
    contactMechanismPurposes: Enumeration[];
    partyContactMechanism: PartyContactMechanism;
    contactMechanismTypes: ContactMechanismType[];
    telecommunicationsNumber: TelecommunicationsNumber;
    m: MetaDomain;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService);
    ngOnInit(): void;
    cancel(): void;
    save(): void;
}
