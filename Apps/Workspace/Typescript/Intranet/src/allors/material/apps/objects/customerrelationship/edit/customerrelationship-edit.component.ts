import * as moment from 'moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, FetcherService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { CustomerRelationship, Organisation, Party } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { ObjectData, SaveService } from '../../../../../material';
import { Meta, FetchServiceEntry } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './customerrelationship-edit.component.html',
  providers: [ContextService]
})
export class CustomerRelationshipEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  partyRelationship: CustomerRelationship;
  internalOrganisation: Organisation;
  party: Party;
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<CustomerRelationshipEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([]) => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.CustomerRelationship({
              object: this.data.id,
              include: {
                InternalOrganisation: x
              }
            }),
            pull.Party({
              object: this.data.associationId,
            }),
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

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.party = loaded.objects.Party as Party;

        if (isCreate) {
          this.title = 'Add Customer Relationship';

          this.partyRelationship = this.allors.context.create('CustomerRelationship') as CustomerRelationship;
          this.partyRelationship.FromDate = moment.utc().toISOString();
          this.partyRelationship.Customer = this.party;
          this.partyRelationship.InternalOrganisation = this.internalOrganisation;
        } else {
          this.partyRelationship = loaded.objects.CustomerRelationship as CustomerRelationship;

          if (this.partyRelationship.CanWriteFromDate) {
            this.title = 'Edit Customer Relationship';
          } else {
            this.title = 'View Customer Relationship';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.partyRelationship.id,
          objectType: this.partyRelationship.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
