import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';
import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { Request } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded } from '../../../../angular';

@Component({
  templateUrl: './requests.component.html',
})
export class RequestsComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: Request[];
  filtered: Request[];

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    public dialogService: TdDialogService,
    public media: TdMediaService,
  ) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle('Requests');
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
        name: 'requests',
        objectType: m.Request,
        include: [
          new TreeNode({ roleType: m.Request.Originator }),
          new TreeNode({ roleType: m.Request.CurrentObjectState }),
        ],
      })];

    this.subscription = this.scope
      .load('Pull', new PullRequest({ query: query }))
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.requests as Request[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  onView(request: Request): void {
    this.router.navigate(['/orders/requests/' + request.id + '/edit']);
  }
}
