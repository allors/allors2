import { AfterViewInit, ChangeDetectorRef, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { PostalAddress, MetaDomain, PartyContactMechanism, Enumeration, Country, Party, PostalBoundary } from "@allors/workspace";
import { WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PartyContactMechanismPostalAddressAddComponent implements OnInit, AfterViewInit, OnDestroy {
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
    contactMechanism: PostalAddress;
    postalBoundary: PostalBoundary;
    contactMechanismPurposes: Enumeration[];
    countries: Country[];
    private subscription;
    private scope;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService, route: ActivatedRoute, media: TdMediaService, changeDetectorRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    save(): void;
    goBack(): void;
}
