import { AfterViewInit, ChangeDetectorRef, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { MetaDomain, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, Party } from "@allors/workspace";
import { WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismTelecommunicationsNumberAddComponent implements OnInit, AfterViewInit, OnDestroy {
    private workspaceService;
    private errorService;
    private route;
    media: TdMediaService;
    private changeDetectorRef;
    title: string;
    subTitle: string;
    m: MetaDomain;
    party: Party;
    partyContactMechanism: PartyContactMechanism;
    contactMechanism: TelecommunicationsNumber;
    contactMechanismPurposes: Enumeration[];
    contactMechanismTypes: ContactMechanismType[];
    private subscription;
    private scope;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService, route: ActivatedRoute, media: TdMediaService, changeDetectorRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    save(): void;
    goBack(): void;
}
