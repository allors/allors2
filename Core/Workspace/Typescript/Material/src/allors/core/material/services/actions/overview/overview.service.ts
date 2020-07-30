import { Injectable } from '@angular/core';

import { Action } from '../../../../../angular';
import { NavigationService } from '../../../../../angular/core/navigation';

import { OverviewAction } from './OverviewAction';

@Injectable({
  providedIn: 'root',
})
export class OverviewService {
  constructor(private navigationService: NavigationService) {}

  overview(): Action {
    return new OverviewAction(this.navigationService);
  }
}
