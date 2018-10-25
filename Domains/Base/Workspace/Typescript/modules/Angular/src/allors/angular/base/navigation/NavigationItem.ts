import { Route } from '@angular/router';
import { RouteData } from '../router/RouteData';
import { MetaPopulation, ObjectType } from '../../../framework';

export class NavigationItem {
  public parent: NavigationItem;
  public children: NavigationItem[];

  public route: Route;
  public link: string;

  public id: string;
  public ids: string[];
  public action: string;

  constructor(metaPopulation: MetaPopulation, navigationItems: NavigationItem[], route: Route, parent?: NavigationItem) {

    navigationItems.push(this);

    this.route = route;
    this.parent = parent;

    if (route.data) {
      const data = route.data as RouteData;
      if (data) {
        this.id = data.id;
        this.action = data.action;
      }
    }

    if (!parent) {
      this.link = route.path;
    } else {
      this.link = route.path ? parent.link + '/' + route.path : parent.link;
    }

    this.children = route.children ? route.children.map((child: Route) => new NavigationItem(metaPopulation, navigationItems, child, this)) : [];

    if (!!this.id) {
      const objectType = metaPopulation.metaObjectById[this.id] as ObjectType;
      this.ids = objectType.classes.map((v) => v.id);
    }
  }
}
