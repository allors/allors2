import { Component, EventEmitter, OnInit, Output, Self } from '@angular/core';

import { ErrorService, Saved, ContextService, MetaService } from '../../../../../angular';
import { Organisation } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'organisation-inline',
  templateUrl: './organisation-inline.component.html',
})
export class OrganisationInlineComponent implements OnInit {

  @Output()
  public saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public organisation: Organisation;

  public m: Meta;

  constructor(
    private allors: ContextService,
    public metaService: MetaService,
    private errorService: ErrorService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    this.allors.context
      .load('Pull', new PullRequest({}))
      .subscribe((loaded) => {
        this.organisation = this.allors.context.create('Organisation') as Organisation;
      }, this.errorService.handler);
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.organisation.id);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
