import { Component, OnDestroy, OnInit } from '@angular/core';

import { PanelManagerService } from '@allors/angular/services/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-launcher',
  templateUrl: './launcher.component.html',
  styleUrls: ['./launcher.component.scss']
})
export class AllorsMaterialLauncherComponent implements OnInit, OnDestroy {

  get panels() {
    return this.panelsService.panels.filter((v) => v.expandable);
  }

  constructor(public panelsService: PanelManagerService) {
  }

  public ngOnInit(): void {
  }

  public ngOnDestroy(): void {
  }
}
