import { Observable, Subscription } from 'rxjs/Rx';
import { Component, OnInit, OnDestroy } from '@angular/core';

import { MetaDomain } from '../../allors/meta';
import { PullRequest, Query, Equals, Like, TreeNode, Sort, Page } from '../../allors/domain';
import { Scope } from '../../allors/angular/base/Scope';
import { AllorsService } from '../allors.service';

import { Organisation } from '../../allors/domain/generated/Organisation.g';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, OnDestroy {

  private scope: Scope;
  private subscription: Subscription;

  m: MetaDomain;
  organisation: Organisation;

  constructor(private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
    this.m = allors.meta;
  }

  ngOnInit() {
    const m = this.allors.meta;
    this.refresh();
  }

  protected refresh(): void {
    this.scope.session.reset();

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m = this.allors.meta;

    const query = new Query({
      name: 'organisations',
      objectType: m.Organisation
    });

    this.scope
      .load('Pull', new PullRequest({ query: [query] }))
      .subscribe(() => {
        this.organisation = (this.scope.collections.organisations as Organisation[])[0];
      },
      (e) => {
        this.allors.onError(e);
      });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {
    this.scope.save().subscribe(() => {
      alert('saved');
    });
  }
}
