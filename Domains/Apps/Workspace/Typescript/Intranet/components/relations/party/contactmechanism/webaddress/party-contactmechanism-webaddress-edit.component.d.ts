import { AfterViewInit, ChangeDetectorRef, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { MetaDomain, PartyContactMechanism, WebAddress, Enumeration } from "@allors/workspace";
import { WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismEditWebAddressComponent implements OnInit, AfterViewInit, OnDestroy {
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
    contactMechanism: WebAddress;
    contactMechanismPurposes: Enumeration[];
    constructor(workspaceService: WorkspaceService, errorService: ErrorService, route: ActivatedRoute, media: TdMediaService, changeDetectorRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    save(): void;
    goBack(): void;
}
