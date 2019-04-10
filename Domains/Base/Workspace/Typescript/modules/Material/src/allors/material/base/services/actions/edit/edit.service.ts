import { Injectable } from '@angular/core';

import { RoleType } from './../../../../../framework';
import { RefreshService } from '../../../../../angular';
import { ObjectService } from '../../../../../material/base/services/object';

import { EditAction } from './EditAction';

@Injectable({
  providedIn: 'root',
})
export class EditService {

  constructor(
    private objectService: ObjectService,
    private refreshService: RefreshService,
    ) {}

  edit(roleType?: RoleType) {
    return new EditAction(this.objectService, this.refreshService, roleType);
  }

}
