import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./ordersOverview.component.html",
})
export class OrdersOverviewComponent implements AfterViewInit {
  public title: string = "Orders Dashboard";

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
    this.titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
