import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { MenuItem, MenuService } from "../../../angular";

@Component({
  template: `
<td-layout-manage-list #manageList [opened]="media.registerQuery('gt-sm') | async" [mode]="(media.registerQuery('gt-sm') | async) ? 'side' :  'over'"
  [sidenavWidth]="(media.registerQuery('gt-xs') | async) ? '257px' : '100%'">
  <mat-toolbar td-sidenav-content>
    <span>Orders</span>
  </mat-toolbar>
  <mat-nav-list td-sidenav-content [tdLayoutManageListClose]="!media.query('gt-sm')">
    <ng-template let-item let-last="last" ngFor [ngForOf]="pages">
      <a mat-list-item [routerLink]="item.link" routerLinkActive="active-link">
        <mat-icon mat-list-icon>{{item.icon}}</mat-icon> {{item.title}}
      </a>
    </ng-template>
  </mat-nav-list>
  <router-outlet></router-outlet>
</td-layout-manage-list>
`,
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
