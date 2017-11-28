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
const platform_browser_1 = require("@angular/platform-browser");
const core_2 = require("@covalent/core");
let DashboardComponent = class DashboardComponent {
    constructor(media, titleService) {
        this.media = media;
        this.titleService = titleService;
        this.title = "Relations Dashboard";
        this.titleService.setTitle(this.title);
    }
};
DashboardComponent = __decorate([
    core_1.Component({
        template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button><mat-icon>settings</mat-icon></button>
  </div>
</mat-toolbar>

<mat-card>
  <mat-card-title>Blah Blah</mat-card-title>
  <mat-card-subtitle>blah blah blah blah</mat-card-subtitle>
  <mat-divider></mat-divider>
  <mat-card-content>
    more blah blah
  </mat-card-content>
</mat-card>
`,
    }),
    __metadata("design:paramtypes", [core_2.TdMediaService, platform_browser_1.Title])
], DashboardComponent);
exports.DashboardComponent = DashboardComponent;
