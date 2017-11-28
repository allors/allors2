import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./dashboard.component.html",
})
export class DashboardComponent implements AfterViewInit {

  public title: string = "Catalogue Dashboard";

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
    this.titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
