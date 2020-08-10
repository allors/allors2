import { PathResolver } from '../Helpers';

import { Reference } from './Reference';

export class Route {
  path: string;
  pathMatch: string;
  component: Reference;
  redirectTo: string;
  outlet: string;
  data: any;
  children: Route[];

  constructor(route: any, pathResolver: PathResolver) {
    this.path = route.path;
    this.pathMatch = route.pathMatch;
    this.component = Reference.fromSymbol(route.component, pathResolver);
    this.redirectTo = route.redirectTo;
    this.outlet = route.outlet;
    this.data = route.data;
    this.children = route.children ? route.children.map((v) => new Route(v, pathResolver)) : undefined;
  }


  public toJSON(): any {

    const { path, pathMatch, component, redirectTo, outlet, data, children } = this;

    return {
      kind: 'route',
      path,
      pathMatch,
      component,
      redirectTo,
      outlet,
      data,
      children
    };
  }
}



