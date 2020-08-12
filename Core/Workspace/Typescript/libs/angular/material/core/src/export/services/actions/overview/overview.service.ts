import { Injectable } from '@angular/core';

import { OverviewAction } from './OverviewAction';
import { NavigationService, Action } from '@allors/angular/core';

@Injectable({
  providedIn: 'root',
})
export class OverviewService {
  constructor(private navigationService: NavigationService) {}

  overview(): Action {
    return new OverviewAction(this.navigationService);
  }
}
