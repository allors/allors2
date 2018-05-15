import { AfterViewInit, ChangeDetectorRef, Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  templateUrl: './relations.component.html',
})
export class RelationsComponent implements AfterViewInit {

  public pages = [ {title: 'Organisations', icon: 'building', link: 'organisations'}, {title: 'People', icon: 'people', link: 'people'}];

  constructor(public activatedRoute: ActivatedRoute) {
  }

  public ngAfterViewInit(): void {
  }
}
