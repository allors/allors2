import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  template: `
<mat-card>
  <mat-card-title>Work Efforts Dashboard</mat-card-title>
  <mat-card-subtitle>Overview</mat-card-subtitle>
  <mat-divider></mat-divider>
  <mat-card-content>
    Info
  </mat-card-content>
</mat-card>
`,
})
export class WorkEffortsOverviewComponent implements AfterViewInit {
  public title: string = "Relations Dashboard";

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
      this.titleService.setTitle(this.title);
  }

  public ngAfterViewInit(): void {
      this.media.broadcast();
      this.changeDetectorRef.detectChanges();
  }
}
