import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Saved, Allors } from '../../../../../../angular';
import { CustomerRelationship, Employment, Enumeration, InternalOrganisation, Locale, Organisation, OrganisationContactKind, OrganisationContactRelationship, Person, PersonRole } from '../../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { Fetcher } from '../../../Fetcher';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  templateUrl: './person-add.component.html',
  providers: [Allors]
})
export class PersonAddComponent implements OnInit, OnDestroy {

  public readonly m: MetaDomain;

  public readonly title = 'Add a new Person';

  public internalOrganisation: InternalOrganisation;
  public person: Person;
  public organisation: Organisation;
  public organisations: Organisation[];
  public organisationContactRelationship: OrganisationContactRelationship;

  public locales: Locale[];
  public genders: Enumeration[];
  public salutations: Enumeration[];
  public organisationContactKinds: OrganisationContactKind[];

  public roles: PersonRole[];
  public selectableRoles: PersonRole[] = [];
  public activeRoles: PersonRole[] = [];
  public customerRelationship: CustomerRelationship;
  public employment: Employment;

  private customerRole: PersonRole;
  private employeeRole: PersonRole;
  private isActiveCustomer: boolean;
  private isActiveEmployee: boolean;
  private subscription: Subscription;

  private readonly refresh$: BehaviorSubject<Date>;
  private readonly fetcher: Fetcher;

  constructor(
    @Self() public allors: Allors,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private titleService: Title,
    private location: Location,
    private stateService: StateService,
    public dialogRef: MatDialogRef<PersonAddComponent>,
  ) {

    this.m = allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, allors.pull);
    this.titleService.setTitle(this.title);
  }

  public ngOnInit(): void {

    const { scope, m, pull } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const organisationId: string = this.route.snapshot.paramMap.get('organisationId');

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
            pull.GenderType({
              predicate: new Equals(m.GenderType.IsActive, true),
              sort: new Sort(m.GenderType.Name),
            }),
            pull.Salutation({
              predicate: new Equals(m.Salutation.IsActive, true),
              sort: new Sort(m.Salutation.Name),
            }),
            pull.PersonRole({
              sort: new Sort(m.PersonRole.Name)
            }),
            pull.OrganisationContactKind({
              sort: new Sort(m.OrganisationContactKind.Description),
            }),
            pull.Organisation({
              sort: new Sort(m.Organisation.PartyName)
            })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;
        this.organisations = loaded.collections.Organisations as Organisation[];
        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.locales = loaded.collections.Locales as Locale[];
        this.genders = loaded.collections.GenderTypes as Enumeration[];
        this.salutations = loaded.collections.Salutations as Enumeration[];
        this.roles = loaded.collections.PersonRoles as PersonRole[];
        this.organisationContactKinds = loaded.collections.OrganisationContactKinds as OrganisationContactKind[];

        this.person = scope.session.create('Person') as Person;

      },
        (error: any) => {
          this.errorService.handle(error);
          this.location.back();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    const { scope } = this.allors;

    if (this.activeRoles.indexOf(this.customerRole) > -1 && !this.isActiveCustomer) {
      const customerRelationship = scope.session.create('CustomerRelationship') as CustomerRelationship;
      customerRelationship.Customer = this.person;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.customerRole) > -1 && this.customerRelationship) {
      this.customerRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.customerRole) === -1 && this.isActiveCustomer) {
      this.customerRelationship.ThroughDate = new Date();
    }

    if (this.activeRoles.indexOf(this.employeeRole) > -1 && !this.isActiveEmployee) {
      const employment = scope.session.create('Employment') as Employment;
      employment.Employee = this.person;
      employment.Employer = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.employeeRole) > -1 && this.employment) {
      this.employment.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.employeeRole) === -1 && this.isActiveEmployee) {
      this.employment.ThroughDate = new Date();
    }

    if (this.organisationContactRelationship === undefined && this.organisation !== undefined) {
      const organisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      organisationContactRelationship.Contact = this.person;
      organisationContactRelationship.Organisation = this.organisation;
    }

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.location.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
