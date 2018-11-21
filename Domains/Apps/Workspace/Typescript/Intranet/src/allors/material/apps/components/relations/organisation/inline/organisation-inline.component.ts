import { Component, EventEmitter, OnInit, Output, Self } from '@angular/core';

import { ErrorService, Saved, SessionService } from '../../../../../../angular';
import { Organisation } from '../../../../../../domain';
import { PullRequest } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';

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

  public m: MetaDomain;

  constructor(
    private allors: SessionService,
    private errorService: ErrorService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    this.allors
      .load('Pull', new PullRequest({}))
      .subscribe((loaded) => {
        this.organisation = this.allors.session.create('Organisation') as Organisation;
      },this.errorService.handler);
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {

    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.organisation.id);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
