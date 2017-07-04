import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Currency, Party, RequestForQuote } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../../angular';

@Component({
  templateUrl: './request.component.html',
})
export class RequestFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  flex: string = '1 1 30rem';
  m: MetaDomain;

  request: RequestForQuote;
  parties: Party[];
  currencies: Currency[];

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService) {

    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'requestForQuote',
            id: id,
            include: [
              new TreeNode({ roleType: m.Request.Originator }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'parties',
              objectType: this.m.Party,
            }),
          new Query(
            {
              name: 'currencies',
              objectType: this.m.Currency,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.request = loaded.objects.requestForQuote as RequestForQuote;
        if (!this.request) {
          this.request = this.scope.session.create('RequestForQuote') as RequestForQuote;
        }

        this.parties = loaded.collections.parties as Party[];
        this.currencies = loaded.collections.currencies as Currency[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  goBack(): void {
    window.history.back();
  }
}
