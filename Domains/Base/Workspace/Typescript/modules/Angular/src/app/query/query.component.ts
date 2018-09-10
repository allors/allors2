import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs/Rx';

import { Loaded, Scope, WorkspaceService } from '../../allors/angular';
import { Organisation } from '../../allors/domain';
import { Like, PullRequest, Pull, Sort, Filter, Result, Fetch } from '../../allors/framework';
import { TreeFactory, MetaDomain } from '../../allors/meta';

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

  constructor(private title: Title, private workspaceService: WorkspaceService) {
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

    const metaPopulation = this.workspaceService.metaPopulation;
    const m = metaPopulation.metaDomain as MetaDomain;
    const tree = new TreeFactory(metaPopulation);

    // tslint:disable:object-literal-sort-keys
    const pulls = [
      new Pull(
        {
          extent: new Filter({
            objectType: m.Organisation,
            predicate: new Like(
              {
                roleType: m.Organisation.Name,
                value: 'Org%',
              }),
            sort: [
              new Sort(
                {
                  roleType: m.Organisation.Name,
                })],
          }),
          results: [
            new Result({
              name: 'organisations',
              fetch: new Fetch({
                include: tree.Organisation(
                  {
                    Owner: {},
                  }),
              }),
              skip: this.skip || 0,
              take: this.take || 10,
            })
          ],
        })];

    this.scope.session.reset();
    this.subscription = this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded: Loaded) => {
        this.organisations = loaded.collections.organisations as Organisation[];
        this.organisationCount = loaded.values.organisations_count;
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
