import { Subject } from 'rxjs';

import { Action, ActionTarget, RefreshService } from '../../../../../angular';

import { ISessionObject, RoleType } from '../../../../../framework';
import { ObjectService } from '../../object';

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
    let editObject = target as ISessionObject

    if (this.roleType) {
      editObject = editObject.get(this.roleType);
    }

    return editObject;
  }

  disabled(target: ActionTarget) {
    var editObject = this.resolve(target);
    return !this.objectService.hasEditControl(editObject);
  }

  execute(target: ActionTarget) {
    var editObject = this.resolve(target);
    this.objectService.edit(editObject)
      .subscribe((v) => {
        this.refreshService.refresh();
        this.result.next(true);
      });
  }
}
