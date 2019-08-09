import { Injectable } from '@angular/core';

import { NavigationService, Action } from '../../../../../angular';

import { OverviewAction } from './OverviewAction';

@Injectable()
export class OverviewService {

  constructor(
    private navigationService: NavigationService
  ) { }

  overview(): Action {
    return new OverviewAction(this.navigationService);
  }
}
