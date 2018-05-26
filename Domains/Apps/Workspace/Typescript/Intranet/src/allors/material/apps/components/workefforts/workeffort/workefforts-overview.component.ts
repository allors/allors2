import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './workefforts-overview.component.html',
})
export class WorkEffortsOverviewComponent {
  public title = 'Relations Dashboard';

  constructor(private dialogService: AllorsMaterialDialogService, private titleService: Title) {
      this.titleService.setTitle(this.title);
  }
}
