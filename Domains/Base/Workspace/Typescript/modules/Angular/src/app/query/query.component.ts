import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Observable, Subject, Subscription } from 'rxjs';

import { Loaded, Scope, WorkspaceService, DataService, x } from '../../allors/angular';
import { Organisation, Person } from '../../allors/domain';
import { Equals, Like, PullRequest, Pull, Sort, TreeNode } from '../../allors/framework';
import { PullFactory, MetaDomain } from '../../allors/meta';

@Component({
  templateUrl: './query.component.html',
})
export class QueryComponent implements OnInit, OnDestroy {

  public organisations: Organisation[];

  public organisationCount: number;
  public skip = 5;
  public take = 5;

  private scope: Scope;
  private subscription: Subscription;

  constructor(
    private title: Title,
    private data: DataService,
    private workspaceService: WorkspaceService
  ) {
    this.scope = workspaceService.createScope();
  }

  public ngOnInit() {
    this.title.setTitle('Query');
    this.query();
  }

  public query() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { m, pull } = this.data;

    const pulls = [
      pull.Organisation({
        predicate: new Like(
          {
            roleType: m.Organisation.Name,
            value: 'Org%',
          }),
        include: { Owner: x },
        sort: new Sort(m.Organisation.Name),
        skip: this.skip || 0,
        take: this.take || 10,
      })
    ];

    this.scope.session.reset();
    this.subscription = this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded: Loaded) => {
        this.organisations = loaded.collections.Organisations as Organisation[];
        this.organisationCount = loaded.values.Organisations_count;
      },
        (error) => {
          alert(error);
        });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
