import { AfterViewInit, ChangeDetectorRef, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { MetaDomain, Enumeration, OrganisationContactRelationship, PersonRole } from "@allors/workspace";
import { WorkspaceService, ErrorService, Filter } from "@allors/base-angular";
export declare class OrganisationContactrelationshipEditComponent implements OnInit, AfterViewInit, OnDestroy {
    private workspaceService;
    private errorService;
    private route;
    media: TdMediaService;
    private changeDetectorRef;
    title: string;
    subTitle: string;
    m: MetaDomain;
    peopleFilter: Filter;
    organisationContactRelationship: OrganisationContactRelationship;
    organisationContactKinds: Enumeration[];
    roles: PersonRole[];
    private subscription;
    private scope;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService, route: ActivatedRoute, media: TdMediaService, changeDetectorRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    save(): void;
    goBack(): void;
}
