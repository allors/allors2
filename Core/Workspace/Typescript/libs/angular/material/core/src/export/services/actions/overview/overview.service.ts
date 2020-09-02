import { Injectable } from '@angular/core';

import { OverviewAction } from './OverviewAction';
import { Action } from '@allors/angular/core';
import { NavigationService } from '@allors/angular/services/core';

@Injectable({
  providedIn: 'root',
})
export class OverviewService {
  constructor(private navigationService: NavigationService) {}

  overview(): Action {
    return new OverviewAction(this.navigationService);
  }
}
