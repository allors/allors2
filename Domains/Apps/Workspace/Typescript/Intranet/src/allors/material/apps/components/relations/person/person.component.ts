import { Component, OnDestroy , OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, LayoutService } from '../../../../../angular';
import { CustomerRelationship, Employment, Enumeration, InternalOrganisation, Locale, Organisation, OrganisationContactKind, OrganisationContactRelationship, Person, PersonRole } from '../../../../../domain';
import { And, Equals, Exists, Fetch, Not, Path, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './person.component.html',
})
export class PersonComponent implements OnInit, OnDestroy {

  public title = 'Person';
  public subTitle: string;

  public m: MetaDomain;

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
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  private fetcher: Fetcher;

  constructor(
    public layout: LayoutService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private titleService: Title,
    private location: Location,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.titleService.setTitle(this.title);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {
    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const organisationId: string = this.route.snapshot.paramMap.get('organisationId');

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const fetches: Fetch[] = [
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Person.Picture }),
            ],
            name: 'person',
          }),
          new Fetch({
            id: organisationId,
            name: 'organisation',
          }),
          new Fetch({
            id,
            name: 'organisationContactRelationships',
            path: new Path({ step: this.m.Person.OrganisationContactRelationshipsWhereContact }),
          }),
        ];

        const queries: Query[] = [
          new Query(
            {
              name: 'locales',
              objectType: this.m.Locale,
              sort: [
                new Sort({ roleType: m.Locale.Name, direction: 'Asc' }),
              ],
          }),
          new Query(
            {
              name: 'genderTypes',
              objectType: this.m.GenderType,
              predicate: new Equals({ roleType: m.GenderType.IsActive, value: true }),
              sort: [
                new Sort({ roleType: m.GenderType.Name, direction: 'Asc' }),
              ],
          }),
          new Query(
            {
              name: 'salutations',
              objectType: this.m.Salutation,
              predicate: new Equals({ roleType: m.Salutation.IsActive, value: true }),
              sort: [
                new Sort({ roleType: m.Salutation.Name, direction: 'Asc' }),
              ],
          }),
          new Query(
            {
              name: 'personRoles',
              objectType: this.m.PersonRole,
              sort: [
                new Sort({ roleType: m.PersonRole.Name, direction: 'Asc' }),
              ],
          }),
          new Query(
            {
              name: 'organisationContactKinds',
              objectType: this.m.OrganisationContactKind,
              sort: [
                new Sort({ roleType: m.OrganisationContactKind.Description, direction: 'Asc' }),
              ],
          }),
        ];

        if (id != null) {
          const customerRelationshipPredicate: And = new And();
          const customerRelationshipPredicates: Predicate[] = customerRelationshipPredicate.predicates;

          customerRelationshipPredicates.push(new Equals({ roleType: m.CustomerRelationship.Customer, value: id }));
          customerRelationshipPredicates.push(new Equals({ roleType: m.CustomerRelationship.InternalOrganisation, value: internalOrganisationId }));
          const not1 = new Not();
          customerRelationshipPredicates.push(not1);

          not1.predicate = new Exists({ roleType: m.CustomerRelationship.ThroughDate });

          const employmentPredicate: And = new And();
          const employmentPredicates: Predicate[] = employmentPredicate.predicates;

          employmentPredicates.push(new Equals({ roleType: m.Employment.Employee, value: id }));
          employmentPredicates.push(new Equals({ roleType: m.Employment.Employer, value: internalOrganisationId }));
          const not2 = new Not();
          employmentPredicates.push(not2);
          not2.predicate = new Exists({ roleType: m.Employment.ThroughDate });

          queries.push(new Query(
            {
              name: 'customerRelationships',
              objectType: m.CustomerRelationship,
              predicate: customerRelationshipPredicate,
            }),
          );

          queries.push(new Query(
            {
              name: 'employments',
              objectType: m.Employment,
              predicate: employmentPredicate,
          }),
          );
        }

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }))
          .switchMap((loaded) => {
            this.scope.session.reset();

            this.subTitle = 'edit person';
            this.person = loaded.objects.person as Person;
            this.organisation = loaded.objects.organisation as Organisation;
            this.internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

            if (this.person) {
              this.customerRelationship = loaded.collections.customerRelationships[0] as CustomerRelationship;
              this.employment = loaded.collections.employments[0] as Employment;
            } else {
              this.subTitle = 'add a new person';
              this.person = this.scope.session.create('Person') as Person;
            }

            this.locales = loaded.collections.locales as Locale[];
            this.genders = loaded.collections.genderTypes as Enumeration[];
            this.salutations = loaded.collections.salutations as Enumeration[];
            this.roles = loaded.collections.personRoles as PersonRole[];
            this.organisationContactKinds = loaded.collections.organisationContactKinds as OrganisationContactKind[];

            if(loaded.collections.organisationContactRelationships && loaded.collections.organisationContactRelationships.length > 0) {
              this.organisationContactRelationship = loaded.collections.organisationContactRelationships[0] as OrganisationContactRelationship;
              this.organisation = this.organisationContactRelationship.Organisation;
            }

            this.customerRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'B29444EF-0950-4D6F-AB3E-9C6DC44C050F');
            this.employeeRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'DB06A3E1-6146-4C18-A60D-DD10E19F7243');
            this.selectableRoles.push(this.customerRole);
            this.selectableRoles.push(this.employeeRole);

            if (this.internalOrganisation.ActiveCustomers.includes(this.person)) {
              this.isActiveCustomer = true;
              this.activeRoles.push(this.customerRole);
            }

            if (this.internalOrganisation.ActiveEmployees.includes(this.person)) {
              this.isActiveEmployee = true;
              this.activeRoles.push(this.employeeRole);
            }

            const organisationQuery: Query[] = [];
            if (this.organisationContactRelationship === undefined && this.organisation === undefined) {
              organisationQuery.push(
                new Query(
                  {
                    name: 'organisations',
                    objectType: m.Organisation,
                    sort: [new Sort({ roleType: m.Organisation.PartyName, direction: 'Asc' })],
                  }),
              );
            }
            return this.scope.load('Pull', new PullRequest({ queries: organisationQuery }));
          });
      })
      .subscribe((loaded) => {
        this.organisations = loaded.collections.organisations as Organisation[];
      },
      (error: any) => {
         this.errorService.handle(error);
         this.goBack();
      },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    if (this.activeRoles.indexOf(this.customerRole) > -1 && !this.isActiveCustomer) {
      const customerRelationship = this.scope.session.create('CustomerRelationship') as CustomerRelationship;
      customerRelationship.Customer = this.person;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(this.customerRole) > -1  && this.customerRelationship) {
      this.customerRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(this.customerRole) === -1 && this.isActiveCustomer) {
      this.customerRelationship.ThroughDate = new Date();
    }

    if (this.activeRoles.indexOf(this.employeeRole) > -1 && !this.isActiveEmployee) {
      const employment = this.scope.session.create('Employment') as Employment;
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
      const organisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      organisationContactRelationship.Contact = this.person;
      organisationContactRelationship.Organisation = this.organisation;
    }

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public goBack(): void {
    this.location.back();
  }
}
