import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { CustomerRelationship, Employment, Enumeration, InternalOrganisation, Locale, Organisation, OrganisationContactKind, OrganisationContactRelationship, Person, PersonRole } from '../../../../../domain';
import { And, Equals, Exists, Fetch, Not, Predicate, PullRequest, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './person.component.html',
})
export class PersonComponent implements OnInit, OnDestroy {

  public title = 'Person';
  public subTitle: string;

  public m: MetaDomain;

  public loaded: boolean;

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
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private titleService: Title,
    private location: Location,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.titleService.setTitle(this.title);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.dataService);
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const organisationId: string = this.route.snapshot.paramMap.get('organisationId');

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Person({
              object: id,
              include: { Picture: x, }
            }),
            pull.Organisation({
              object: organisationId,
            }),
            pull.Person({
              object: id,
              fetch: {
                OrganisationContactRelationshipsWhereContact: x
              }
            }),
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
            pull.GenderType({
              predicate: new Equals(m.GenderType.IsActive),
              sort: new Sort(m.GenderType.Name),
            }),
            pull.Salutation({
              predicate: new Equals({ propertyType: m.Salutation.IsActive, value: true }),
              sort: new Sort(m.Salutation.Name),
            }),
            pull.PersonRole({
              sort: new Sort(m.PersonRole.Name)
            }),
            pull.OrganisationContactKind({
              sort: new Sort(m.OrganisationContactKind.Description),
            })
          ];

          if (id != null) {
            const customerRelationshipPredicate: And = new And();
            const customerRelationshipPredicates: Predicate[] = customerRelationshipPredicate.operands;

            customerRelationshipPredicates.push(new Equals({ propertyType: m.CustomerRelationship.Customer, value: id }));
            customerRelationshipPredicates.push(new Equals({ propertyType: m.CustomerRelationship.InternalOrganisation, value: internalOrganisationId }));
            const not1 = new Not();
            customerRelationshipPredicates.push(not1);

            not1.operand = new Exists({ propertyType: m.CustomerRelationship.ThroughDate });

            const employmentPredicate: And = new And();
            const employmentPredicates: Predicate[] = employmentPredicate.operands;

            employmentPredicates.push(new Equals({ propertyType: m.Employment.Employee, value: id }));
            employmentPredicates.push(new Equals({ propertyType: m.Employment.Employer, value: internalOrganisationId }));
            const not2 = new Not();
            employmentPredicates.push(not2);
            not2.operand = new Exists({ propertyType: m.Employment.ThroughDate });

            pulls.push(
              pull.CustomerRelationship({
                predicate: customerRelationshipPredicate,
              })
            );

            pulls.push(
              pull.Employment({
                predicate: employmentPredicate,
              })
            );
          }

          return this.scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
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

                if (loaded.collections.organisationContactRelationships && loaded.collections.organisationContactRelationships.length > 0) {
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

                const organisationQuery = [];
                if (this.organisationContactRelationship === undefined && this.organisation === undefined) {
                  organisationQuery.push(
                    pull.Organisation({
                      sort: new Sort(m.Organisation.PartyName)
                    })
                  );
                }
                return this.scope.load('Pull', new PullRequest({ pulls: organisationQuery }));
              })

            );
        })
      )
      .subscribe((loaded) => {
        this.organisations = loaded.collections.organisations as Organisation[];
        this.loaded = true;
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

    if (this.activeRoles.indexOf(this.customerRole) > -1 && this.customerRelationship) {
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
