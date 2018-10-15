import { Route } from '@angular/router';
import { RouteData } from '../router/RouteData';

export class MenuItem {
  public route: Route;
  public parent: MenuItem;
  public children: MenuItem[];

  public type: 'module' | 'page';
  public title: string;
  public icon?: string;

  public link: string;

  get module(): MenuItem {
    if (this.isModule) {
      return this;
    }

    return this.parent ? this.parent.module : undefined;
  }

  get isModule(): boolean {
    return this.type === 'module';
  }

  get isPage(): boolean {
    return this.type === 'page';
  }

  constructor(menuItemByRoute: Map<Route, MenuItem>, modules: MenuItem[], pagesByModule: Map<MenuItem, MenuItem[]>, route: Route, parent?: MenuItem, module?: MenuItem) {
    menuItemByRoute.set(route, this);

    this.parent = parent;
    if (route.data) {
      const data = route.data as RouteData;
      this.type = data.menuType;
      this.title = data.title || route.path;
      this.icon = data.icon;
    }

    if (!parent) {
      this.link = route.path;
    } else {
      this.link = route.path ? parent.link + '/' + route.path : parent.link;
    }

    if (this.isModule) {
      modules.push(this);
      pagesByModule.set(this, []);
    }

    if (this.isPage) {
      pagesByModule.get(module).push(this);
    }

    this.children = route.children ? route.children.map((child: Route) => new MenuItem(menuItemByRoute, modules, pagesByModule, child, this, this.isModule ? this : module)) : [];
  }
}
