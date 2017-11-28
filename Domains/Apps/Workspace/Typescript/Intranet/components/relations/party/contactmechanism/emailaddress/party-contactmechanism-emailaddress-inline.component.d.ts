import { EventEmitter, OnInit } from "@angular/core";
import { EmailAddress, MetaDomain, PartyContactMechanism, ContactMechanismPurpose } from "@allors/workspace";
import { Scope, WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismEmailAddressInlineComponent implements OnInit {
    private workspaceService;
    private errorService;
    saved: EventEmitter<PartyContactMechanism>;
    cancelled: EventEmitter<any>;
    scope: Scope;
    emailAddress: EmailAddress;
    partyContactMechanism: PartyContactMechanism;
    contactMechanismPurposes: ContactMechanismPurpose[];
    m: MetaDomain;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService);
    ngOnInit(): void;
    cancel(): void;
    save(): void;
}
