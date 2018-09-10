import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Rx';

import { Loaded, Scope, WorkspaceService } from '../../allors/angular';
import { Organisation } from '../../allors/domain';
import { Fetch, PullRequest, Pull } from '../../allors/framework';
import { Result } from '../../allors/framework/database/data/Result';
import { TreeFactory, PathFactory, MetaDomain } from '../../allors/meta';

@Component({
  templateUrl: './fetch.component.html',
})
export class FetchComponent implements OnInit, OnDestroy {

  public organisation: Organisation;
  public organisations: Organisation[];

  private scope: Scope;
  private subscription: Subscription;

  constructor(
    private title: Title,
    private route: ActivatedRoute,
    private workspaceService: WorkspaceService) {

    this.scope = workspaceService.createScope();
  }

  public ngOnInit() {
    this.title.setTitle('Fetch');
    this.fetch();
  }

  public fetch() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const metaPopulation = this.workspaceService.metaPopulation;
    const m = metaPopulation.metaDomain as MetaDomain;
    const tree = new TreeFactory(metaPopulation);
    const path = new PathFactory(metaPopulation);

    const id = this.route.snapshot.paramMap.get('id');

    // tslint:disable:object-literal-sort-keys
    const pulls = [
      new Pull(
        {
          object: id,
          results: [
            new Result({
              name: 'organisation',
              fetch: new Fetch({
                include: tree.Organisation({
                  Owner: {}
                })
              })
            }),
            new Result({
              name: 'organisations',
              fetch: new Fetch({
                path: path.Organisation({
                  Owner: {
                    OrganisationsWhereOwner: {}
                  }
                })
              })
            })
          ]
        }),
    ];

    this.scope.session.reset();
    this.subscription = this.scope
      .load('Pull', new PullRequest({
        pulls,
      }))
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.organisation as Organisation;
        this.organisations = loaded.collections.organisations as Organisation[];
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
