import { Component, EventEmitter, OnInit, Output, Self, OnDestroy } from '@angular/core';

import {  Saved, ContextService, MetaService } from '../../../../../angular';
import { Organisation } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'organisation-inline',
  templateUrl: './organisation-inline.component.html',
})
export class OrganisationInlineComponent implements OnInit, OnDestroy {

  @Output()
  public saved: EventEmitter<Organisation> = new EventEmitter<Organisation>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public organisation: Organisation;

  public m: Meta;

  constructor(
    private allors: ContextService,
    public metaService: MetaService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    this.allors.context
      .load('Pull', new PullRequest({}))
      .subscribe((loaded) => {
        this.organisation = this.allors.context.create('Organisation') as Organisation;
      });
  }

  public ngOnDestroy(): void {
    if (!!this.organisation) {
      this.allors.context.delete(this.organisation);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
      this.saved.emit(this.organisation);
      this.organisation = undefined;
  }
}
