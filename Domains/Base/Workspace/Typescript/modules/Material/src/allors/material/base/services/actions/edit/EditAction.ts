import { Subject } from 'rxjs';

import { Action, ActionTarget, RefreshService } from '../../../../../angular';

import { EditService } from './edit.service';
import { ISessionObject } from '../../../../../framework';
import { ObjectService } from '../../object';

export class EditAction implements Action {

  result = new Subject<boolean>();
  name = () => 'Edit';
  description = () => 'Edit';

  constructor(
    private objectService: ObjectService,
    private refreshService: RefreshService
  ) {
  }

  disabled(target: ActionTarget) {
    return !this.objectService.hasEditControl(target as ISessionObject);
  }

  execute(target: ActionTarget) {
    this.objectService.edit(target as ISessionObject)
      .subscribe((v) => {
        this.refreshService.refresh();
        this.result.next(true);
      });
  }
}
