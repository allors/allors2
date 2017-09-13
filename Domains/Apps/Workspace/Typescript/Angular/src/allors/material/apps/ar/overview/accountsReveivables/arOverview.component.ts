import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./arOverview.component.html",
})
export class ArOverviewComponent implements AfterViewInit {
  public title: string = "Relations Dashboard";

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
      this.titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {
      this.media.broadcast();
      this.changeDetectorRef.detectChanges();
  }
}
