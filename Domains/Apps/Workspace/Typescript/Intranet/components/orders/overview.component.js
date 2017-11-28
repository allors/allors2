"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const router_1 = require("@angular/router");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
let OverviewComponent = class OverviewComponent {
    constructor(media, changeDetectorRef, menu, activatedRoute) {
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.menu = menu;
        this.activatedRoute = activatedRoute;
        this.pages = [];
        this.pages = menu.pages(activatedRoute.routeConfig);
    }
    ngAfterViewInit() {
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
};
OverviewComponent = __decorate([
    core_1.Component({
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
    }),
    __metadata("design:paramtypes", [core_2.TdMediaService, core_1.ChangeDetectorRef, base_angular_1.MenuService, router_1.ActivatedRoute])
], OverviewComponent);
exports.OverviewComponent = OverviewComponent;
