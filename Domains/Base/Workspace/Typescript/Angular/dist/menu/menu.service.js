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
require("rxjs/add/operator/filter");
require("rxjs/add/operator/map");
const MenuItem_1 = require("./MenuItem");
let MenuService = class MenuService {
    constructor(router) {
        this.router = router;
        this.menuItemByRoute = new Map();
        this.modules = [];
        this.pagesByModule = new Map();
        this.menuItems = this.router.config.map((route) => new MenuItem_1.MenuItem(this.menuItemByRoute, this.modules, this.pagesByModule, route));
    }
    pages(route) {
        const menuItem = this.menuItemByRoute.get(route);
        const module = menuItem.module;
        return this.pagesByModule.get(module);
    }
};
MenuService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [router_1.Router])
], MenuService);
exports.MenuService = MenuService;
