
import { ActionTarget } from '../../../../angular';

import { NavigateService } from './navigate.service';

export class NavigateEditAction {

  name = () => 'Edit';

  description = (target: ActionTarget) => `Edit ${target.object.toString()}`;

  handler = (target: ActionTarget) => {
    this.navigateService.navigationService.edit(target.object);
  }

  constructor(private navigateService: NavigateService) {
  }
}
