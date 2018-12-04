import { Component, OnDestroy, OnInit, Self, SkipSelf } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ErrorService, Saved, SessionService, NavigationService, AllorsPanelService, RefreshService, MetaService } from '../../../../../../angular';
import { CustomerRelationship, Employment, Enumeration, InternalOrganisation, Locale, Organisation, OrganisationContactKind, OrganisationContactRelationship, Person, PersonRole } from '../../../../../../domain';
import { And, Equals, Exists, Not, PullRequest, Sort } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-personal-panel',
  templateUrl: './person-personal-panel.component.html',
  providers: [AllorsPanelService, SessionService]
})
export class PersonPersonalPanelComponent implements OnInit, OnDestroy {

  readonly m: MetaDomain;

  person: Person;

  internalOrganisation: InternalOrganisation;
  organisation: Organisation;
  organisations: Organisation[];
  organisationContactRelationship: OrganisationContactRelationship;
  locales: Locale[];
  genders: Enumeration[];
  salutations: Enumeration[];
  organisationContactKinds: OrganisationContactKind[];
  roles: PersonRole[];
  selectableRoles: PersonRole[] = [];
  activeRoles: PersonRole[] = [];
  customerRelationship: CustomerRelationship;
  employment: Employment;

  private minimizedSubscription: Subscription;
  private maximizedSubscription: Subscription;

  constructor(
    @SkipSelf() public shared: SessionService,
    @Self() public local: SessionService,
    @Self() public panelService: AllorsPanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService) {

    this.m = this.metaService.m;

    panelService.name = 'edit';
    panelService.title = 'Personal Data';
    panelService.icon = 'person';
    panelService.maximizable = true;
  }

  public ngOnInit(): void {

    this.initMinimized();
    this.initMaximized();
  }

  public ngOnDestroy(): void {
    if (this.minimizedSubscription) {
      this.minimizedSubscription.unsubscribe();
    }

    if (this.maximizedSubscription) {
      this.maximizedSubscription.unsubscribe();
    }
  }

  public save(): void {

    const customerRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'B29444EF-0950-4D6F-AB3E-9C6DC44C050F');
    const employeeRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'DB06A3E1-6146-4C18-A60D-DD10E19F7243');
    const isActiveCustomer = this.internalOrganisation.ActiveCustomers.includes(this.person);
    const isActiveEmployee = this.internalOrganisation.ActiveEmployees.includes(this.person);

    if (this.activeRoles.indexOf(customerRole) > -1 && !isActiveCustomer) {
      const customerRelationship = this.local.session.create('CustomerRelationship') as CustomerRelationship;
      customerRelationship.Customer = this.person;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(customerRole) > -1 && this.customerRelationship) {
      this.customerRelationship.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(customerRole) === -1 && isActiveCustomer) {
      this.customerRelationship.ThroughDate = new Date();
    }

    if (this.activeRoles.indexOf(employeeRole) > -1 && !isActiveEmployee) {
      const employment = this.local.session.create('Employment') as Employment;
      employment.Employee = this.person;
      employment.Employer = this.internalOrganisation;
    }

    if (this.activeRoles.indexOf(employeeRole) > -1 && this.employment) {
      this.employment.ThroughDate = null;
    }

    if (this.activeRoles.indexOf(employeeRole) === -1 && isActiveEmployee) {
      this.employment.ThroughDate = new Date();
    }

    if (this.organisationContactRelationship === undefined && this.organisation !== undefined) {
      const organisationContactRelationship = this.local.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      organisationContactRelationship.Contact = this.person;
      organisationContactRelationship.Organisation = this.organisation;
    }

    this.local
      .save()
      .subscribe((saved: Saved) => {
        this.location.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  private initMinimized() {
    const personPullName = `${this.panelService.name}_${this.m.Person.objectType.name}`;

    this.minimizedSubscription = combineLatest(
      this.route.url,
      this.route.queryParams,
      this.refreshService.refresh$,
    )
      .pipe(
        filter((v) => {
          return this.panelService.isMinimized;
        }),
        switchMap(() => {

          this.person = undefined;

          const { pull, x } = this.metaService;
          const id = this.panelService.panelsService.id;

          const pulls = [
            pull.Person({
              name: personPullName,
              object: id,
              include: {
                GeneralEmail: x,
                PersonalEmailAddress: x,
              }
            })
          ];

          return this.local
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.person = loaded.objects[personPullName] as Person;
      }, this.errorService.handler);
  }

  private initMaximized() {
    this.maximizedSubscription = combineLatest(
      this.route.url,
      this.route.queryParams,
      this.refreshService.refresh$,
      this.stateService.internalOrganisationId$
    )
      .pipe(
        filter((v) => {
          return this.panelService.isMaximized;
        }),
        switchMap(([, , , internalOrganisationId]) => {

          this.person = undefined;

          const { m, pull, x } = this.metaService;
          const fetcher = new Fetcher(this.stateService, this.metaService.pull);
          const id = this.panelService.panelsService.id;

          const pulls = [
            fetcher.internalOrganisation,
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
            pull.GenderType({
              predicate: new Equals({ propertyType: m.GenderType.IsActive, value: true }),
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
            })          ,
              pull.Person({
                object: id,
                include: {
                  Picture: x,
                }
              }),
              pull.Person({
                object: id,
                fetch: {
                  OrganisationContactRelationshipsWhereContact: x
                }
              }),
              pull.CustomerRelationship({
                predicate: new And({
                  operands: [
                    new Equals({ propertyType: m.CustomerRelationship.Customer, object: id }),
                    new Equals({ propertyType: m.CustomerRelationship.InternalOrganisation, object: internalOrganisationId }),
                    new Not({
                      operand: new Exists({ propertyType: m.CustomerRelationship.ThroughDate }),
                    })
                  ]
                }),
              }),
              pull.Employment({
                predicate: new And({
                  operands: [
                    new Equals({ propertyType: m.Employment.Employee, object: id }),
                    new Equals({ propertyType: m.Employment.Employer, object: internalOrganisationId }),
                    new Not({
                      operand: new Exists({ propertyType: m.Employment.ThroughDate })
                    })
                  ]
                }),
              })
            ];

          return this.local
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.local.session.reset();

        this.person = loaded.objects.Person as Person;
        this.organisations = loaded.collections.Organisations as Organisation[];
        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.genders = loaded.collections.GenderTypes as Enumeration[];
        this.salutations = loaded.collections.Salutations as Enumeration[];
        this.roles = loaded.collections.PersonRoles as PersonRole[];
        this.organisationContactKinds = loaded.collections.OrganisationContactKinds as OrganisationContactKind[];

        [this.customerRelationship] = loaded.collections.CustomerRelationships as CustomerRelationship[];
        [this.employment] = loaded.collections.Employments as Employment[];
        [this.organisationContactRelationship] = loaded.collections.OrganisationContactRelationships as OrganisationContactRelationship[];
        this.organisation = this.organisationContactRelationship.Organisation;

        this.selectableRoles = [];
        const customerRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'B29444EF-0950-4D6F-AB3E-9C6DC44C050F');
        const employeeRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'DB06A3E1-6146-4C18-A60D-DD10E19F7243');
        this.selectableRoles.push(customerRole);
        this.selectableRoles.push(employeeRole);

        this.activeRoles = [];
        if (this.internalOrganisation.ActiveCustomers.includes(this.person)) {
          this.activeRoles.push(customerRole);
        }

        if (this.internalOrganisation.ActiveEmployees.includes(this.person)) {
          this.activeRoles.push(employeeRole);
        }
      }, this.errorService.handler);
  }
}
