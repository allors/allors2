import { Subject } from 'rxjs';

import { RoleType } from '@allors/meta/system';
import { ISessionObject } from '@allors/domain/system';
import { ActionTarget, Action } from '@allors/angular/core';
import { ObjectService } from '@allors/angular/material/services/core';

import { RefreshService } from '@allors/angular/services/core';

export class EditAction implements Action {

  name = 'edit';
  result = new Subject<boolean>();
  displayName = () => 'Edit';
  description = () => 'Edit';

  constructor(
    private objectService: ObjectService,
    private refreshService: RefreshService,
    private roleType?: RoleType
  ) {
  }

  resolve(target: ActionTarget) {
    let editObject = target as ISessionObject;

    if (this.roleType) {
      editObject = editObject.get(this.roleType);
    }

    return editObject;
  }

  disabled(target: ActionTarget) {
    let editObject = this.resolve(target);
    return !this.objectService.hasEditControl(editObject);
  }

  execute(target: ActionTarget) {
    let editObject = this.resolve(target);
    this.objectService.edit(editObject)
      .subscribe(() => {
        this.refreshService.refresh();
        this.result.next(true);
      });
  }
}
