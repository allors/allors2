import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Observable, Subject, Subscription } from 'rxjs';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../angular';
import { Person } from '../../../../domain';
import { Equals, Like, PullRequest, Sort, TreeNode } from '../../../../framework';
import { MetaDomain } from '../../../../meta';
import { PullFactory } from '../../../../meta/generated/pull.g';

@Component({
  templateUrl: './people.component.html',
})
export class PeopleComponent implements AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;
  public data: Person[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router) {

    this.title = 'People';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public goBack(): void {
    this.router.navigate(['/']);
  }

  public ngAfterViewInit(): void {
    this.search();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public search(criteria?: string): void {

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const pull = new PullFactory(this.workspaceService.metaPopulation);
    const pulls = [pull.Person()];

    this.scope.session.reset();

    this.subscription = this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.people as Person[];
      },
      (error: any) => {
        alert(error);
        this.goBack();
      });
  }

  public delete(person: Person): void {
    /* this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this person?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      }); */
  }

  public onView(person: Person): void {
    this.router.navigate(['/relations/people/' + person.id + '/overview']);
  }
}
