import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject, Subscription } from 'rxjs';

import { Loaded, Scope, WorkspaceService } from '../../allors/angular';
import { Organisation } from '../../allors/domain';
import { Equals, Fetch, Like, Path, PullRequest, Pull, Sort, TreeNode } from '../../allors/framework';
import { PullFactory, TreeFactory } from '../../allors/meta';

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

    const x = {};
    const pull = new PullFactory(this.workspaceService.metaPopulation);
    const tree = new TreeFactory(this.workspaceService.metaPopulation);

    const id = this.route.snapshot.paramMap.get('id');

    // tslint:disable:object-literal-sort-keys
    const pulls = [
      pull.Organisation({ object: id, include: { Owner: x } }),
      pull.Organisation({ object: id, path: { Owner: { OrganisationsWhereOwner: x } }, include: tree.Organisation({ Owner: x }) })
    ];

    this.scope.session.reset();
    this.subscription = this.scope
      .load('Pull', new PullRequest({
        pulls,
      }))
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.Organisation as Organisation;
        this.organisations = loaded.collections.Organisations as Organisation[];
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
