import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { Organisation } from '../../allors/domain';
import { PullRequest, Query, MetaObjectType } from '../../allors/framework';
import { MetaDomain } from '../../allors/meta';

import { Scope, WorkspaceService } from '../../allors/angular';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public organisation: Organisation;

  private scope: Scope;
  private subscription: Subscription;

  constructor(private workspaceService: WorkspaceService) {
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public ngOnInit() {
    this.refresh();
  }

  public save(): void {
    this.scope.save().subscribe(() => {
      alert('saved');
    });
  }

  protected refresh(): void {
    this.scope.session.reset();

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const queries = [new Query({
      name: 'organisations',
      objectType: this.m.Organisation as MetaObjectType,
    })];

    this.scope
      .load('Pull', new PullRequest({ queries }))
      .subscribe((loaded) => {
        this.organisation = (loaded.collections.organisations as Organisation[])[0];
      },
      (e) => {
        // TODO:
        // this.allors.onError(e);
      });
  }
}
