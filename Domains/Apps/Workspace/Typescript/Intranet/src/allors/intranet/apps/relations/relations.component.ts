import { Component, Input, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './relations.component.html',
})
export class RelationsComponent implements AfterViewInit {

  pages = [
    {title: 'Organisations', icon: 'building', link: 'organisations'},
    {title: 'People', icon: 'people', link: 'people'}];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, public activatedRoute: ActivatedRoute) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
