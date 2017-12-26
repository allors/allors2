import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./dashboard.component.html",
})
export class DashboardComponent {

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {
  }
}
