import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';

import { NavigationItem } from './NavigationItem';
import { ISessionObject, ObjectType, MetaObject, MetaObjectType } from 'src/allors/framework';

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

  overview(sessionObject: ISessionObject) {

    const objectTypeId = sessionObject.objectType.id;
    const navigationItem = this.navigationItems.find((v) => v.id === objectTypeId && v.action === 'overview');
    const url = navigationItem.route.path.replace(`:id`, sessionObject.id);
    this.router.navigate([url]);
  }

  add(objectTypeOrMetaObjectType: ObjectType | MetaObjectType, ...params: ISessionObject[]) {
    const objectTypeId = (objectTypeOrMetaObjectType instanceof ObjectType) ? objectTypeOrMetaObjectType.id : objectTypeOrMetaObjectType._objectType.id;
    const navigationItem = this.navigationItems.find((v) => v.id === objectTypeId && v.action === 'add');
    const url = navigationItem.route.path;
    const queryParams = params.reduce((acc, v) => { acc[v.objectType.name] = v.id; return acc; }, {});
    this.router.navigate([url], { queryParams });
  }

  edit(sessionObject: ISessionObject) {
    const objectTypeId = sessionObject.objectType.id;
    const navigationItem = this.navigationItems.find((v) => v.id === objectTypeId && v.action === 'edit');
    const url = navigationItem.route.path.replace(`:id`, sessionObject.id);
    this.router.navigate([url]);
  }

  private flatten(route: Route, acc: Route[]) {
    acc.push(route);
    if (route.children) {
      route.children.forEach((v) => this.flatten(v, acc));
    }
  }

}
