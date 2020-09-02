import { Subject } from 'rxjs';

import { ActionTarget, Action } from '@allors/angular/core';
import { NavigationService } from '@allors/angular/services/core';

function objectTypeName(target: ActionTarget) {
  return Array.isArray(target) ? (target.length > 0) && target[0].objectType.name : target.objectType.name;
}

export class OverviewAction implements Action {

  name = 'overview';

  constructor(private navigationService: NavigationService) {
  }

  result = new Subject<boolean>();

  displayName = (target: ActionTarget) => 'Overview';

  description = (target: ActionTarget) => `Go to ${objectTypeName(target)} overview`;

  disabled = () => false;

  execute = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      if (target.length > 0) {
        this.navigationService.overview(target[0]);
      }
    } else {
      this.navigationService.overview(target);
    }

    this.result.next(true);
  }
}
