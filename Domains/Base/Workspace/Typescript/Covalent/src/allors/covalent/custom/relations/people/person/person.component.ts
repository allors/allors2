import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Organisation, Person, Locale, Enumeration } from '../../../../../domain';
import { Scope, Loaded, Saved, AllorsService, ErrorService } from '../../../../../angular';

@Component({
  templateUrl: './person.component.html',
})
export class PersonComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  title: string;

  m: MetaDomain;
  locales: Locale[];
  person: Person;

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    private media: TdMediaService) {


    this.title = 'Person';
    this.titleService.setTitle(this.title);
    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');

        const m: MetaDomain = this.allorsService.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'person',
            id: id,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'locales',
              objectType: this.m.Locale,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.person = loaded.objects.person as Person;
        if (!this.person) {
          this.person = this.scope.session.create('Person') as Person;
        }

        this.locales = loaded.collections.locales as Locale[];
      },
      (error: any) => {
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
