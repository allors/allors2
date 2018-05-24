import { Input, Component } from '@angular/core';
import { SideNavService } from '../../services/sidenav/sidenav.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-sidenavtoggle',
  templateUrl: './sidenavtoggle.component.html',
})
export class SideNavToggleComponent {

  constructor(private sideNavService: SideNavService) {
  }

  public toggle(){
    this.sideNavService.toggle();
  }
}
