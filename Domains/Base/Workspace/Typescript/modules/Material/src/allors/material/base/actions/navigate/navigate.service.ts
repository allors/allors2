import { Injectable } from '@angular/core';

import { NavigationService, Action } from '../../../../angular';

import { NavigateOverviewAction } from './NavigateOverviewAction';
import { NavigateEditAction } from './NavigateEditAction';

@Injectable()
export class NavigateService {

  constructor(
    public navigationService: NavigationService
  ) { }

  overview(): Action {
    return new NavigateOverviewAction(this);
  }

  edit(): Action {
    return new NavigateEditAction(this);
  }

}
