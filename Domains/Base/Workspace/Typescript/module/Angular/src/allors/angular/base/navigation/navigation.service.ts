import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { ISessionObject, ObjectType, } from '../../../framework';

@Injectable()
export class NavigationService {

  constructor(private router: Router) { }

  list(objectType: ObjectType) {
    const url = objectType.list;
    this.router.navigate([url]);
  }

  overview(sessionObject: ISessionObject) {
    const url = sessionObject.objectType.overview.replace(`:id`, sessionObject.id);
    this.router.navigate([url]);
  }

  back(): any {
    throw new Error('Remove this method');
  }
}
