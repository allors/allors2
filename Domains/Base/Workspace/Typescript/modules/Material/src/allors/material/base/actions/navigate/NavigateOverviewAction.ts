import { MethodType } from '../../../../framework';
import { Deletable } from '../../../../domain';
import { ActionTarget, SessionService, Invoked, ErrorService } from '../../../../angular';

import { NavigateService } from './navigate.service';

export class NavigateOverviewAction {

  name = (target: ActionTarget) => 'Overview';

  description = (target: ActionTarget) => `Go to ${target.object.objectType.name} overview`;

  handler = (target: ActionTarget) => {
    this.navigateService.navigationService.overview(target.object);
  }

  constructor(private navigateService: NavigateService) {
  }
}
