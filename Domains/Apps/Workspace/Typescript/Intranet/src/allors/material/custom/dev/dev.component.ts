import { Component, OnDestroy, OnInit, Self } from '@angular/core';

import { Subscription, timer, } from 'rxjs';
import { switchMap, take } from 'rxjs/operators';

import { PullRequest } from '../../../framework';
import { AllorsFilterService, ContextService, NavigationService, RefreshService, MetaService, AllorsBarcodeService } from '../../../angular';

import { Task } from '../../../domain';

import { ObjectService } from '../../base/services/object';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-dev',
  templateUrl: './dev.component.html',
})
export class DevComponent {

  constructor(
    public allors: ContextService,
    public metaService: MetaService,
    public barcodeService: AllorsBarcodeService,
    public navigation: NavigationService) {
  }

  sendBarcode() {
    const code = 'P1';

    this.barcodeService.simulateSubject.next(code);
  }
}
