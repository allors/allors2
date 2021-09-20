import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { IObject } from '@allors/domain/system';
import { ObjectType } from '@allors/meta/system';

import { NavigationService } from '@allors/angular/services/core';

@Injectable({
  providedIn: 'root',
})
export class NavigationServiceCore extends NavigationService {
  constructor(private router: Router) {
    super();
  }

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
