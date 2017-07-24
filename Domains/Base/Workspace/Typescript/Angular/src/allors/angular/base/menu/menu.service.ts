import { Injectable , ChangeDetectorRef } from '@angular/core';
import { Router, Route, NavigationEnd, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';

import { MenuItem } from './MenuItem';

@Injectable()
export class MenuService {
  menuItems: MenuItem[];
  menuItemByRoute: Map<Route, MenuItem>;

  modules: MenuItem[];
  pagesByModule: Map<MenuItem, MenuItem[]>;

  constructor(private router: Router, private titleService: Title) {

    this.menuItemByRoute = new Map();
    this.modules = [];
    this.pagesByModule = new Map();

    this.menuItems = this.router.config.map((route: Route) => new MenuItem(this.menuItemByRoute, this.modules, this.pagesByModule, route));
  }

  pages(route: Route): MenuItem[] {
    const menuItem: MenuItem = this.menuItemByRoute.get(route);
    const module: MenuItem =  menuItem.module;
    return this.pagesByModule.get(module);
  }
}
