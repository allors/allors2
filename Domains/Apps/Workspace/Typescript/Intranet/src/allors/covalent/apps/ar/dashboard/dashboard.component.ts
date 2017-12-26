import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./dashboard.component.html",
})
export class DashboardComponent {
  public title: string = "Dashboard";

  constructor(public media: TdMediaService, private readonly titleService: Title) {
      this.titleService.setTitle(this.title);
  }
}
