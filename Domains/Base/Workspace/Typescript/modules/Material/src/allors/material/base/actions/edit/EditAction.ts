import { Subject } from 'rxjs';

import { Action, ActionTarget } from '../../../../angular';

import { EditService } from './edit.service';
import { ISessionObject } from '../../../../framework';

export class EditAction implements Action {

  result = new Subject<boolean>();
  name = () => 'Edit';
  description = () => 'Edit';

  constructor(private editService: EditService) {
  }

  disabled(target: ActionTarget) {
    return !this.editService.objectService.hasEditControl(target as ISessionObject);
  }

  execute(target: ActionTarget) {
    this.editService.objectService.edit(target as ISessionObject)
      .subscribe((v) => {
        this.editService.refreshService.refresh();
        this.result.next(true);
      });
  }
}
