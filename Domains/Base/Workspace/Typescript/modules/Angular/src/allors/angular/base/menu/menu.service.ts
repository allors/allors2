import { ChangeDetectorRef , Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Route, Router } from '@angular/router';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { MenuItem } from './MenuItem';

@Injectable()
export class MenuService {
  public menuItems: MenuItem[];
  public menuItemByRoute: Map<Route, MenuItem>;

  public modules: MenuItem[];
  public pagesByModule: Map<MenuItem, MenuItem[]>;

  constructor(private router: Router) {

    this.menuItemByRoute = new Map();
    this.modules = [];
    this.pagesByModule = new Map();

    this.menuItems = this.router.config.map((route: Route) => new MenuItem(this.menuItemByRoute, this.modules, this.pagesByModule, route));
  }

  public pages(route: Route): MenuItem[] {
    const menuItem: MenuItem = this.menuItemByRoute.get(route);
    const module: MenuItem =  menuItem.module;
    return this.pagesByModule.get(module);
  }
}
