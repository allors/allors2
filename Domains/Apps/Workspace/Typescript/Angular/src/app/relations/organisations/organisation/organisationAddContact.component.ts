import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { Scope } from '../../../../allors/angular/base/Scope';
import { AllorsService } from '../../../allors.service';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../allors/domain';
import { MetaDomain } from '../../../../allors/meta/index';

import { Organisation, Person, Locale, OrganisationContactRelationship, OrganisationContactKind, Enumeration } from '../../../../allors/domain';

@Component({
  templateUrl: './organisationContact.component.html',
})
export class OrganisationAddContactComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  m: MetaDomain;

  organisationContactRelationship: OrganisationContactRelationship;
  person: Person;
  locales: Locale[];
  genders: Enumeration[];
  salutations: Enumeration[];
  organisationContactKinds: Enumeration[];

  constructor(private allors: AllorsService,
    private route: ActivatedRoute,
    public snackBar: MdSnackBar,
    public media: TdMediaService) {
    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'organisation',
            id: id,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'organisationContactKinds',
              objectType: this.m.OrganisationContactKind,
            }),
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
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe(() => {

        if (!this.person) {
          this.person = this.scope.session.create('Person') as Person;
          this.organisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
        }

        const organisation: Organisation = this.scope.objects.organisation as Organisation;

        this.organisationContactRelationship.Organisation = organisation;
        this.organisationContactRelationship.Contact = this.person;

        this.locales = this.scope.collections.locales as Locale[];
        this.genders = this.scope.collections.genders as Enumeration[];
        this.salutations = this.scope.collections.salutations as Enumeration[];
        this.organisationContactKinds = this.scope.collections.organisationContactKinds as Enumeration[];
      },
      (error: any) => {
        this.snackBar.open(error, 'close', { duration: 5000 });
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
      .subscribe((pushResponse: PushResponse) => {
        this.goBack();
      },
      (e: any) => {
        this.snackBar.open(e.toString(), 'close', { duration: 5000 });
      });
  }

  goBack(): void {
    window.history.back();
  }
}
