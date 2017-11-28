import { AfterViewInit, ChangeDetectorRef } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { MenuItem, MenuService } from "@allors/base-angular";
export declare class OverviewComponent implements AfterViewInit {
    media: TdMediaService;
    private changeDetectorRef;
    activatedRoute: ActivatedRoute;
    menu: MenuService;
    pages: MenuItem[];
    constructor(media: TdMediaService, changeDetectorRef: ChangeDetectorRef, activatedRoute: ActivatedRoute, menu: MenuService);
    ngAfterViewInit(): void;
}
