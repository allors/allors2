import { Component, EventEmitter, OnInit , Output } from '@angular/core';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, LayoutService } from '../../../../../angular';
import { Enumeration, Locale, Person, Organisation } from '../../../../../domain';
import { PullRequest, Query, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
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

  private scope: Scope;

  constructor(
    public layout: LayoutService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    this.scope
      .load('Pull', new PullRequest({ }))
      .subscribe((loaded) => {
        this.organisation = this.scope.session.create('Organisation') as Organisation;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.organisation.id);
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }
}
