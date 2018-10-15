import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';

import { NavigationItem } from './NavigationItem';
import { ISessionObject } from 'src/allors/framework';

@Injectable()
export class NavigationService {
  public navigationItems: NavigationItem[];

  constructor(private router: Router) {
    const routes: Route[] = [];
    this.router.config.forEach((v) => this.flatten(v, routes));

    this.navigationItems = routes
      .filter(v => v.data && v.data.id)
      .map((route: Route) => new NavigationItem(route));
  }

  overview(...sessionObjects: ISessionObject[]) {

    const objectTypeId = sessionObjects[0].objectType.id;
    const navigationItem = this.navigationItems.find((v) => v.id === objectTypeId && v.action === 'overview');
    const url = sessionObjects.reduce((acc, v, i) => acc.replace(`:${i}` , v.id) , navigationItem.route.path);
    this.router.navigate([url]);
  }

  private flatten(route: Route, acc: Route[]) {
    acc.push(route);
    if (route.children) {
      route.children.forEach((v) => this.flatten(v, acc));
    }
  }

}
