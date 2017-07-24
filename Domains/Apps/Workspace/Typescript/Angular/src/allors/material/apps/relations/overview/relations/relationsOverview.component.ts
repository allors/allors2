import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './relationsOverview.component.html',
})
export class RelationsOverviewComponent implements AfterViewInit {
  title: string = 'Relations Dashboard';

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
      this.titleService.setTitle(this.title);
  }

  ngAfterViewInit(): void {
      this.media.broadcast();
      this.changeDetectorRef.detectChanges();
  }
}
