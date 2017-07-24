import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy , ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { Organisation, Person, PersonRole, Locale, Enumeration } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../angular';

@Component({
  templateUrl: './form.component.html',
})
export class PersonEditComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  title: string = 'Person';
  subTitle: string;

  m: MetaDomain;

  person: Person;

  locales: Locale[];
  genders: Enumeration[];
  salutations: Enumeration[];
  roles: PersonRole[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'person',
            id: id,
            include: [
              new TreeNode({ roleType: m.Person.Picture }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'locales',
              objectType: this.m.Locale,
            }),
          new Query(
            {
              name: 'genders',
              objectType: this.m.GenderType,
            }),
          new Query(
            {
              name: 'salutations',
              objectType: this.m.Salutation,
            }),
          new Query(
            {
              name: 'roles',
              objectType: this.m.PersonRole,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.subTitle = 'edit person';
        this.person = loaded.objects.person as Person;

        if (!this.person) {
          this.subTitle = 'add a new person';
          this.person = this.scope.session.create('Person') as Person;
        }

        this.locales = loaded.collections.locales as Locale[];
        this.genders = loaded.collections.genders as Enumeration[];
        this.salutations = loaded.collections.salutations as Enumeration[];
        this.roles = loaded.collections.roles as PersonRole[];
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
