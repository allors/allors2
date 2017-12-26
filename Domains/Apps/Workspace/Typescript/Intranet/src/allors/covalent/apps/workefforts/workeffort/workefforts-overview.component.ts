import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./workefforts-overview.component.html",
})
export class WorkEffortsOverviewComponent {
  public title: string = "Relations Dashboard";

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
      this.titleService.setTitle(this.title);
  }
}
