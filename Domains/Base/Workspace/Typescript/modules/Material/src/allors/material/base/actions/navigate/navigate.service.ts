import { Injectable } from '@angular/core';

import { NavigationService } from '../../../../angular';

import { NavigateOverviewAction } from './NavigateOverviewAction';
import { NavigateEditAction } from './NavigateEditAction';

@Injectable()
export class NavigateService {

  constructor(
    public navigationService: NavigationService
  ) { }

  overview() {
    return new NavigateOverviewAction(this);
  }

  edit() {
    return new NavigateEditAction(this);
  }

}
