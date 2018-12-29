import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Enumeration, TelecommunicationsNumber, ElectronicAddress, ContactMechanism, PartyContactMechanism } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { EditData, CreateData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './partycontactmechanism-edit.component.html',
  providers: [ContextService]
})
export class PartyContactmechanismEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  partyContactMechanism: PartyContactMechanism;
  contactMechanismPurposes: Enumeration[];
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<PartyContactmechanismEditComponent>,
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

          const pulls = [
            pull.PartyContactMechanism({
              object: this.data.id,
              include: {
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x
                  }
                }
              },
            }),
            // pull.Person({
            //   object: this.data.id,
            //   fetch: {
            //     CurrentOrganisationContactRelationships: {
            //     }
            //   }
            // }),
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(this.m.ContactMechanismPurpose.Name)
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];

        if (isCreate) {
          this.title = 'Add Party ContactMechanism';

          this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
        } else {
          this.partyContactMechanism = loaded.objects.PartyContactMechanism as PartyContactMechanism;

          if (this.partyContactMechanism.CanWriteComment) {
            this.title = 'Edit Party ContactMechanism';
          } else {
            this.title = 'View Party ContactMechanism';
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
