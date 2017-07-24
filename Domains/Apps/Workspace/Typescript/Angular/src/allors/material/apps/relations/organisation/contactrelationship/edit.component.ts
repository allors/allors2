import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Organisation, Person, PersonRole, Locale, OrganisationContactRelationship, OrganisationContactKind, Enumeration } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Filter, Invoked } from '../../../../../angular';

@Component({
  templateUrl: './form.component.html',
})
export class OrganisationContactrelationshipEditComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  title: string = 'Contact Relationship';
  subTitle: string = 'add a new contact relationship';

  m: MetaDomain;

  peopleFilter: Filter;

  organisationContactRelationship: OrganisationContactRelationship;
  organisationContactKinds: Enumeration[];
  roles: PersonRole[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;

    this.peopleFilter = new Filter(this.scope, this.m.Person, [this.m.Person.FirstName, this.m.Person.LastName]);
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const roleId: string = this.route.snapshot.paramMap.get('roleId');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'organisationContactRelationship',
            id: roleId,
            include: [
              new TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
            ],
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
              name: 'roles',
              objectType: this.m.PersonRole,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.organisationContactRelationship = loaded.objects.organisationContactRelationship as OrganisationContactRelationship;

        this.organisationContactKinds = loaded.collections.organisationContactKinds as Enumeration[];
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
