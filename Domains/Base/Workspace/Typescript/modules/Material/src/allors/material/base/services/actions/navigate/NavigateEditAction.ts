import { Subject } from 'rxjs';

import { ActionTarget, Action } from '../../../../../angular';

import { NavigateService } from './navigate.service';

export class NavigateEditAction implements Action {

  constructor(private navigateService: NavigateService) {
  }

  result = new Subject<boolean>();

  name = () => 'Edit';

  description = (target: ActionTarget) => `Edit ${target.toString()}`;

  disabled = () => false;

  execute = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      if (target.length > 0) {
        // TODO:
        // this.navigateService.navigationService.edit(target[0]);
      }
    } else {
        // TODO:
        // this.navigateService.navigationService.edit(target);
    }

    this.result.next(true);
  }
}
