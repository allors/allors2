import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta/index';
import { RoleType } from '../../../../../meta';
import { PullRequest, PushResponse, Fetch, Path, Query, And, Or, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Organisation, Person, Locale, Enumeration } from '../../../../../domain';
import { Scope, Filter, Loaded, Saved, AllorsService, ErrorService } from '../../../../../angular';

@Component({
  templateUrl: './organisation.component.html',
})
export class OrganisationComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  title: string;

  m: MetaDomain;
  people: Person[];

  organisation: Organisation;

  peopleFilter: Filter;

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    private media: TdMediaService) {

    this.title = 'Organisation';
    this.titleService.setTitle(this.title);
    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;

    this.peopleFilter = new Filter(this.scope, this.m.Person, [this.m.Person.FirstName, this.m.Person.LastName]);
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');

        const m: MetaDomain = this.allorsService.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'organisation',
            id: id,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'people',
              objectType: this.m.Person,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.organisation = loaded.objects.organisation as Organisation;
        if (!this.organisation) {
          this.organisation = this.scope.session.create('Organisation') as Organisation;
        }

        this.people = loaded.collections.people as Person[];
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

  ownerSelected(person: Person): void {
    console.log(person.displayName);
  }
}
