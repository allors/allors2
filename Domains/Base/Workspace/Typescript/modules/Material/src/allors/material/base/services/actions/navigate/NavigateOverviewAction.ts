import { Subject } from 'rxjs';

import { ActionTarget, Action } from '../../../../../angular';

import { NavigateService } from './navigate.service';

function objectTypeName(target: ActionTarget) {
  return Array.isArray(target) ? (target.length > 0) && target[0].objectType.name : target.objectType.name;
}

export class NavigateOverviewAction implements Action {

  constructor(private navigateService: NavigateService) {
  }

  result = new Subject<boolean>();

  name = (target: ActionTarget) => 'Overview';

  description = (target: ActionTarget) => `Go to ${objectTypeName(target)} overview`;

  disabled = () => false;

  execute = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      if (target.length > 0) {
        this.navigateService.navigationService.overview(target[0]);
      }
    } else {
      this.navigateService.navigationService.overview(target);
    }

    this.result.next(true);
  }
}
