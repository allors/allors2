import { Component, OnDestroy, OnInit, Self, SkipSelf } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, NavigationService, PanelService, RefreshService, MetaService, Context } from '../../../../../../angular';
import { CustomerRelationship, Employment, Enumeration, InternalOrganisation, Locale, Organisation, OrganisationContactKind, OrganisationContactRelationship, Person, PersonRole } from '../../../../../../domain';
import { And, Equals, Exists, Not, PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-overview-detail',
  templateUrl: './person-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class PersonOverviewDetailComponent implements OnInit, OnDestroy {

  readonly m: Meta;

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

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService) {

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Personal Data';
    panel.icon = 'person';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.Person.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.Person({
            name: pullName,
            object: id,
            include: {
              GeneralEmail: x,
              PersonalEmailAddress: x,
            }
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.person = loaded.objects[pullName] as Person;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest(
      this.route.url,
      this.route.queryParams,
      this.refreshService.refresh$,
      this.stateService.internalOrganisationId$,
    )
      .pipe(
        filter((v) => {
          return this.panel.isExpanded;
        }),
        switchMap(([, , , internalOrganisationId]) => {

          this.person = undefined;

          const { m, pull, x } = this.metaService;
          const fetcher = new Fetcher(this.stateService, this.metaService.pull);
          const id = this.panel.manager.id;

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
            }),
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

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.person = loaded.objects.Person as Person;
        this.organisations = loaded.collections.Organisations as Organisation[];
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    const customerRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'B29444EF-0950-4D6F-AB3E-9C6DC44C050F');
    const employeeRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === 'DB06A3E1-6146-4C18-A60D-DD10E19F7243');
    const isActiveCustomer = this.internalOrganisation.ActiveCustomers.includes(this.person);
    const isActiveEmployee = this.internalOrganisation.ActiveEmployees.includes(this.person);

    if (this.activeRoles.indexOf(customerRole) > -1 && !isActiveCustomer) {
      const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
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
      const employment = this.allors.context.create('Employment') as Employment;
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
      const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      organisationContactRelationship.Contact = this.person;
      organisationContactRelationship.Organisation = this.organisation;
    }

    this.allors.context.save()
      .subscribe((saved: Saved) => {
        this.location.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
