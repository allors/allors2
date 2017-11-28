import { AfterViewInit, ChangeDetectorRef, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { MetaDomain, PartyContactMechanism, Enumeration, TelecommunicationsNumber } from "@allors/workspace";
import { WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismTelecommunicationsNumberEditComponent implements OnInit, AfterViewInit, OnDestroy {
    private workspaceService;
    private errorService;
    private route;
    media: TdMediaService;
    private changeDetectorRef;
    private subscription;
    private scope;
    title: string;
    subTitle: string;
    m: MetaDomain;
    partyContactMechanism: PartyContactMechanism;
    contactMechanism: TelecommunicationsNumber;
    contactMechanismPurposes: Enumeration[];
    contactMechanismTypes: Enumeration[];
    constructor(workspaceService: WorkspaceService, errorService: ErrorService, route: ActivatedRoute, media: TdMediaService, changeDetectorRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    save(): void;
    goBack(): void;
}
