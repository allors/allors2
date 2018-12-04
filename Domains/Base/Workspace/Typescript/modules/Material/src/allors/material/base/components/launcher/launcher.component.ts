import { Component, OnDestroy, OnInit } from '@angular/core';
import { PanelsService } from '../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-launcher',
  templateUrl: './launcher.component.html',
  styleUrls: ['./launcher.component.scss']
})
export class AllorsMaterialLauncherComponent implements OnInit, OnDestroy {

  get panels() {
    return this.panelsService.panels.filter((v) => v.maximizable);
  }

  constructor(public panelsService: PanelsService) {
  }

  public ngOnInit(): void {
  }

  public ngOnDestroy(): void {
  }
}
