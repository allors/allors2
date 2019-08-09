import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { IObject, ObjectType } from '../../../framework';

@Injectable()
export class NavigationService {
  constructor(private router: Router) {}

  list(objectType: ObjectType) {
    const url = objectType.list;
    this.router.navigate([url]);
  }

  overview(obj: IObject) {
    const url = obj.objectType.overview.replace(`:id`, obj.id);
    this.router.navigate([url]);
  }
}
