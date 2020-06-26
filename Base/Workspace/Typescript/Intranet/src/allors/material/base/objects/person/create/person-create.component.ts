import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Saved, ContextService, NavigationService, MetaService, InternalOrganisationId, FetcherService, TestScope, SingletonId, RefreshService } from '../../../../../angular';
import { CustomerRelationship, Employment, Enumeration, InternalOrganisation, Locale, Organisation, OrganisationContactKind, OrganisationContactRelationship, Person, PersonRole, Currency } from '../../../../../domain';
import { Equals, PullRequest, Sort, IObject } from '../../../../../framework';
import { ObjectData, SaveService, FiltersService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  templateUrl: './person-create.component.html',
  providers: [ContextService]
})
export class PersonCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  public title = 'Add Person';

  internalOrganisation: InternalOrganisation;
  person: Person;
  organisation: Organisation;
  organisations: Organisation[];
  organisationContactRelationship: OrganisationContactRelationship;

  locales: Locale[];
  genders: Enumeration[];
  salutations: Enumeration[];
  organisationContactKinds: OrganisationContactKind[];
  selectedContactKinds: OrganisationContactKind[] = [];

  roles: PersonRole[];
  selectedRoles: PersonRole[] = [];
  customerRelationship: CustomerRelationship;
  employment: Employment;

  private customerRole: PersonRole;
  private employeeRole: PersonRole;

  private subscription: Subscription;
  private readonly refresh$: BehaviorSubject<Date>;
  currencies: Currency[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PersonCreateComponent>,
    public metaService: MetaService,
    public navigationService: NavigationService,
    public filtersService: FiltersService,
    public location: Location,
    public refreshService: RefreshService,
    private route: ActivatedRoute,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private singletonId: SingletonId,
    private internalOrganisationId: InternalOrganisationId,
  ) {

    super();

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.locales,
            pull.Singleton({
              object: this.singletonId.value,
              fetch: {
                Locales: {
                  include: {
                    Language: x,
                    Country: x
                  }
                }
              }
            }),
            pull.Currency({
              predicate: new Equals({ propertyType: m.Currency.IsActive, value: true }),
              sort: new Sort(m.Currency.Name),
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
            pull.Organisation({
              object: this.data.associationId,
            }),
            pull.Organisation({
              name: 'AllOrganisations',
              sort: new Sort(m.Organisation.PartyName)
            })
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;
        this.organisations = loaded.collections.AllOrganisations as Organisation[];
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.currencies = loaded.collections.Currencies as Currency[];
        this.locales = loaded.collections.Locales as Locale[] || [];
        this.genders = loaded.collections.GenderTypes as Enumeration[];
        this.salutations = loaded.collections.Salutations as Enumeration[];
        this.roles = loaded.collections.PersonRoles as PersonRole[];
        this.organisationContactKinds = loaded.collections.OrganisationContactKinds as OrganisationContactKind[];

        this.customerRole = this.roles.find((v: PersonRole) => v.UniqueId === 'b29444ef-0950-4d6f-ab3e-9c6dc44c050f');
        this.employeeRole = this.roles.find((v: PersonRole) => v.UniqueId === 'db06a3e1-6146-4c18-a60d-dd10e19f7243');

        this.person = this.allors.context.create('Person') as Person;
        this.person.CollectiveWorkEffortInvoice = false;
        this.person.PreferredCurrency = this.internalOrganisation.PreferredCurrency;

      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    if (this.selectedRoles.indexOf(this.customerRole) > -1) {
      const customerRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
      customerRelationship.Customer = this.person;
      customerRelationship.InternalOrganisation = this.internalOrganisation;
    }

    if (this.selectedRoles.indexOf(this.employeeRole) > -1) {
      const employment = this.allors.context.create('Employment') as Employment;
      employment.Employee = this.person;
      employment.Employer = this.internalOrganisation;
    }

    if (this.organisation !== undefined) {
      const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      organisationContactRelationship.Contact = this.person;
      organisationContactRelationship.Organisation = this.organisation;
      organisationContactRelationship.ContactKinds = this.selectedContactKinds;
    }

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.person.id,
          objectType: this.person.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
