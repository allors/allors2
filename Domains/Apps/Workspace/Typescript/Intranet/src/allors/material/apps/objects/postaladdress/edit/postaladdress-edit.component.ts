import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Party, PartyContactMechanism, PostalAddress, Enumeration, PostalBoundary, Country, ContactMechanism } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { EditData, CreateData, ObjectData } from 'src/allors/material/base/services/object';
import * as moment from 'moment';

@Component({
  templateUrl: './postaladdress-edit.component.html',
  providers: [ContextService]
})
export class PostalAddressEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  party: Party;
  partyContactMechanism: PartyContactMechanism;
  contactMechanism: PostalAddress;
  postalBoundary: PostalBoundary;
  contactMechanismPurposes: Enumeration[];
  countries: Country[];
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<PostalAddressEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          let pulls = [
            pull.ContactMechanism({
              object: this.data.id,
              include: {
                PostalAddress_PostalBoundary: {
                  Country: x
                }
              },
            }),
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(m.ContactMechanismPurpose.Name)
            }),
            pull.Country({
              sort: new Sort(m.Country.Name)
            })
          ];

          if (isCreate) {
            pulls = [
              ...pulls,
              pull.Party({
                object: this.data.associationId,
              })
            ];
          }

          // if (!isCreate) {
          //   pulls = [
          //     ...pulls,
          //     pull.ContactMechanism({
          //       object: this.data.id,
          //       fetch: {
          //         part
          //       }
          //     }),
          //   ];
          // }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.party = loaded.objects.Party as Party;
        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];
        this.countries = loaded.collections.Countries as Country[];

        if (isCreate) {
          this.title = 'Add Postal Address';

          this.contactMechanism = this.allors.context.create('PostalAddress') as PostalAddress;

          this.postalBoundary = this.allors.context.create('PostalBoundary') as PostalBoundary;
          this.contactMechanism.PostalBoundary = this.postalBoundary;

          this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
          this.partyContactMechanism.FromDate = moment().toDate();
          this.partyContactMechanism.ContactMechanism = this.contactMechanism;
          this.partyContactMechanism.UseAsDefault = true;

          this.party.AddPartyContactMechanism(this.partyContactMechanism);
        } else {
          this.contactMechanism = loaded.objects.ContactMechanism as PostalAddress;

          if (this.contactMechanism.CanWriteAddress1) {
            this.title = 'Edit Postal Address';
          } else {
            this.title = 'View Postal Address';
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.partyContactMechanism.id,
          objectType: this.partyContactMechanism.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
