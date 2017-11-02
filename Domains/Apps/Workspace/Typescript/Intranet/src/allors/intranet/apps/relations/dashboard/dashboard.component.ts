import { Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./dashboard.component.html",
})
export class DashboardComponent {

  public title: string;

  constructor(public media: TdMediaService, private titleService: Title) {

      this.title = "Dashboard";
      this.titleService.setTitle(this.title);
  }
}
