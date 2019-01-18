import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, ContextService, NavigationService, MetaService } from '../../../../../angular';
import { InternalOrganisation, Locale, WorkTask, Organisation, Party, PartyContactMechanism, Person, ContactMechanism, OrganisationContactRelationship } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './worktask-create.component.html',
  providers: [ContextService]
})
export class WorkTaskCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  public title = 'Add Work Task';

  add: boolean;
  edit: boolean;

  internalOrganisation: InternalOrganisation;
  workTask: WorkTask;
  contactMechanisms: ContactMechanism[];
  contacts: Person[];
  addContactPerson = false;
  addContactMechanism: boolean;

  locales: Locale[];

  private subscription: Subscription;
  private readonly refresh$: BehaviorSubject<Date>;
  private readonly fetcher: Fetcher;
  organisations: Organisation[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<WorkTaskCreateComponent>,
    public metaService: MetaService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(
      this.route.url,
      this.refresh$,
      this.stateService.internalOrganisationId$
    )
      .pipe(
        switchMap(([]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Locale({
              sort: new Sort(m.Locale.Name)
            }),
            pull.Organisation({
              sort: new Sort(m.Organisation.PartyName)
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.organisations = loaded.collections.Organisations as Organisation[];

        this.workTask = this.allors.context.create('WorkTask') as WorkTask;
        this.workTask.TakenBy = this.internalOrganisation as Organisation;

      }, this.errorService.handler);
  }

  public customerSelected(customer: Party) {
    this.updateCustomer(customer);
  }

  private updateCustomer(party: Party) {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x,
                }
              }
            }
          }
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      })
    ];

    this.allors.context.load('Pull', new PullRequest({ pulls })).subscribe(
      (loaded) => {
        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms
          .map((v: PartyContactMechanism) => v.ContactMechanism);

        this.contacts = loaded.collections.CurrentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.handle(error);
      },
    );
  }

  public contactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.workTask.Customer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
    this.workTask.ContactPerson = contact;
  }

  public contactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {

    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.workTask.Customer.AddPartyContactMechanism(partyContactMechanism);
    this.workTask.FullfillContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.workTask.id,
          objectType: this.workTask.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
