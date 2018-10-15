export class RouteData {
  id: string;
  menuType: 'module' | 'page';
  action: string;
  title: string;
  icon: string;
}

export function data(fields: Partial<RouteData>) {
  return fields;
}

export function moduleData(fields: Partial<RouteData>): Partial<RouteData> {
  return Object.assign({ menuType: 'module' }, fields);
}

export function pageListData(fields: Partial<RouteData>): Partial<RouteData> {
  return Object.assign({ menuType: 'page', action: 'list' }, fields);
}

export function overviewData(fields: Partial<RouteData>): Partial<RouteData> {
  return Object.assign({ action: 'overview' }, fields);
}
