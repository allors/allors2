import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';


@Component({
  templateUrl: './workefforts-overview.component.html',
})
export class WorkEffortsOverviewComponent {
  public title = 'Relations Dashboard';

  constructor(private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
      this.titleService.setTitle(this.title);
  }
}
