import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";
export declare class DashboardComponent {
    media: TdMediaService;
    private titleService;
    title: string;
    constructor(media: TdMediaService, titleService: Title);
}
