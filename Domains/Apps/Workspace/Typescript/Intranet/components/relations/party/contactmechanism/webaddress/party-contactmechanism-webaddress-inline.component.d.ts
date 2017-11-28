import { EventEmitter, OnInit } from "@angular/core";
import { MetaDomain, PartyContactMechanism, WebAddress, ContactMechanismPurpose } from "@allors/workspace";
import { Scope, WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismInlineWebAddressComponent implements OnInit {
    private workspaceService;
    private errorService;
    saved: EventEmitter<PartyContactMechanism>;
    cancelled: EventEmitter<any>;
    scope: Scope;
    webAddress: WebAddress;
    partyContactMechanism: PartyContactMechanism;
    contactMechanismPurposes: ContactMechanismPurpose[];
    m: MetaDomain;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService);
    ngOnInit(): void;
    cancel(): void;
    save(): void;
}
