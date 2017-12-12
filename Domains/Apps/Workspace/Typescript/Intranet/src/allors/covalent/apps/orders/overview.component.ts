import { AfterViewInit, ChangeDetectorRef, Component, Input } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { MenuItem, MenuService } from "../../../angular";

@Component({
  templateUrl: "./overview.component.html",
})
export class OverviewComponent implements AfterViewInit {

  public pages: MenuItem[] = [];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, public menu: MenuService, public activatedRoute: ActivatedRoute) {
    this.pages = menu.pages(activatedRoute.routeConfig);
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
