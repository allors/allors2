import { AfterViewInit, ChangeDetectorRef, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { MetaDomain, SalesInvoice } from "@allors/workspace";
import { WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class InvoicePrintComponent implements OnInit, AfterViewInit, OnDestroy {
    private workspaceService;
    private errorService;
    private route;
    media: TdMediaService;
    private changeDetectorRef;
    m: MetaDomain;
    invoice: SalesInvoice;
    body: string;
    private subscription;
    private scope;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService, route: ActivatedRoute, media: TdMediaService, changeDetectorRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    goBack(): void;
}
