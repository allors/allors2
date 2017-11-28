import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./dashboard.component.html",
})
export class DashboardComponent implements AfterViewInit {

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
