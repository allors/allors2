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
    constructor(media, changeDetectorRef, titleService) {
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.titleService = titleService;
        this.title = "Catalogue Dashboard";
        this.titleService.setTitle(this.title);
    }
    ngAfterViewInit() {
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
};
DashboardComponent = __decorate([
    core_1.Component({
        templateUrl: "./dashboard.component.html",
    }),
    __metadata("design:paramtypes", [core_2.TdMediaService, core_1.ChangeDetectorRef, platform_browser_1.Title])
], DashboardComponent);
exports.DashboardComponent = DashboardComponent;
