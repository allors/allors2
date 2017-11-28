import { AfterViewInit, ChangeDetectorRef } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";
export declare class WorkEffortsOverviewComponent implements AfterViewInit {
    media: TdMediaService;
    private changeDetectorRef;
    private titleService;
    title: string;
    constructor(media: TdMediaService, changeDetectorRef: ChangeDetectorRef, titleService: Title);
    ngAfterViewInit(): void;
}
