import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';

import { ContextService, MetaService } from '@allors/angular/services/core';
import { Organisation } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { PullRequest } from '@allors/protocol/system';

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
      .load(new PullRequest({}))
      .subscribe(() => {
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
