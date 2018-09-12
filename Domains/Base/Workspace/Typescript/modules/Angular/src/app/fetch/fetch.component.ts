import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { Loaded, Scope, WorkspaceService, DataService, x } from '../../allors/angular';
import { Organisation } from '../../allors/domain';
import { PullRequest } from '../../allors/framework';

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
    private data: DataService,
    workspace: WorkspaceService
  ) {

    this.scope = workspace.createScope();
  }

  public ngOnInit() {
    this.title.setTitle('Fetch');
    this.fetch();
  }

  public fetch() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { pull, tree } = this.data;
    const id = this.route.snapshot.paramMap.get('id');

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
