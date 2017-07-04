import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { TdLoadingService, TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { Organisation } from '../../../../domain';
import { Scope, Loaded, AllorsService, ErrorService } from '../../../../angular';

@Component({
  templateUrl: './organisations.component.html',
})
export class OrganisationsComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: Organisation[];

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    private dialogService: TdDialogService,
    private loadingService: TdLoadingService) {
    this.scope = new Scope(allorsService.database, allorsService.workspace);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle('Organisations');
    this.search();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  search(criteria?: string): void {

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m: MetaDomain = this.allorsService.meta;

    const query: Query[] = [new Query(
      {
        name: 'organisations',
        objectType: m.Organisation,
        include: [
          new TreeNode({ roleType: m.Organisation.Owner }),
          new TreeNode({ roleType: m.Organisation.Employees }),
        ],
      })];

    this.scope.session.reset();

    this.subscription = this.scope
      .load('Pull', new PullRequest({ query: query }))
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.organisations as Organisation[];
      },
      (error: any) => {
        alert(error);
        this.goBack();
      });
  }

  delete(organisation: Organisation): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this organisation?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(organisation: Organisation): void {
    this.router.navigate(['/relations/organisations/' + organisation.id + '/overview']);
  }
}
