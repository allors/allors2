import { Injectable, ɵangular_packages_core_core_bj } from '@angular/core';
import { Router } from '@angular/router';

import { IObject, ObjectType } from '../../../framework';

@Injectable()
export class NavigationService {
  constructor(private router: Router) {}

  hasList(obj: IObject): boolean {
    return !!obj?.objectType?.list;
  }

  list(objectType: ObjectType) {
    const url = objectType.list;
    this.router.navigate([url]);
  }

  hasOverview(obj: IObject): boolean {
    return !!obj?.objectType?.overview;
  }

  overview(obj: IObject) {
    const url = obj.objectType.overview.replace(`:id`, obj.id);
    this.router.navigate([url]);
  }
}
