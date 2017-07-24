import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy , ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Request } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../../angular';

@Component({
  templateUrl: './request-overview.component.html',
})
export class RequestOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;
  m: MetaDomain;

  title: string = 'Requests Overview';
  request: Request;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {

    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'request',
            id: id,
            include: [
              new TreeNode({ roleType: m.Request.Originator }),
              new TreeNode({ roleType: m.Request.CurrentObjectState }),
              new TreeNode({ roleType: m.Request.CreatedBy }),
              new TreeNode({ roleType: m.Request.LastModifiedBy }),
              new TreeNode({
                roleType: m.Request.RequestStatuses,
                nodes: [
                  new TreeNode({ roleType: m.RequestStatus.RequestObjectState }),
                ],
               }),
              new TreeNode({
                roleType: m.Request.FullfillContactMechanism,
                nodes: [
                  new TreeNode({
                    roleType: m.PostalAddress.PostalBoundary,
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                  }),
                ],
               }),
            ],
          }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch  }));
      })
      .subscribe((loaded: Loaded) => {
        this.request = loaded.objects.request as Request;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  goBack(): void {
    window.history.back();
  }

  checkType(obj: any): string {
    return obj.objectType.name;
  }
}
