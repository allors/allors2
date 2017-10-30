import { AfterViewInit, ChangeDetectorRef, Component, Input } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { MenuItem, MenuService } from "@allors";

@Component({
  templateUrl: "./relations.component.html",
})
export class RelationsComponent implements AfterViewInit {

  public pages: MenuItem[] = [];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, public activatedRoute: ActivatedRoute, public menu: MenuService) {
    this.pages = menu.pages(activatedRoute.routeConfig);
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
