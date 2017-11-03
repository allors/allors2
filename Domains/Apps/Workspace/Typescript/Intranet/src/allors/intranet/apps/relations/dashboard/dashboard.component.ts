import { Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./dashboard.component.html",
})
export class DashboardComponent {

  public title = "Relations Dashboard";

  constructor(public media: TdMediaService, private titleService: Title) {

      this.titleService.setTitle(this.title);
  }
}
