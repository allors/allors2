import { EventEmitter, OnInit } from "@angular/core";
import { PostalAddress, MetaDomain, PartyContactMechanism, Country, ContactMechanismPurpose, PostalBoundary } from "@allors/workspace";
import { Scope, WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismPostalAddressInlineComponent implements OnInit {
    private workspaceService;
    private errorService;
    saved: EventEmitter<PartyContactMechanism>;
    cancelled: EventEmitter<any>;
    scope: Scope;
    partyContactMechanism: PartyContactMechanism;
    postalAddress: PostalAddress;
    postalBoundary: PostalBoundary;
    countries: Country[];
    contactMechanismPurposes: ContactMechanismPurpose[];
    m: MetaDomain;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService);
    ngOnInit(): void;
    cancel(): void;
    save(): void;
}
