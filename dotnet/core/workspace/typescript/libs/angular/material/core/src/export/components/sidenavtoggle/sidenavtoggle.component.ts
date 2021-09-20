import { Input, Component } from '@angular/core';
import { AllorsMaterialSideNavService } from '@allors/angular/material/services/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-sidenavtoggle',
  templateUrl: './sidenavtoggle.component.html',
})
export class AllorsMaterialSideNavToggleComponent {

  constructor(private sideNavService: AllorsMaterialSideNavService) {
  }

  public toggle() {
    this.sideNavService.toggle();
  }
}
