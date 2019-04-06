import * as moment from 'moment';

import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import {  ContextService, MetaService, RefreshService } from '../../../../../angular';
import { SupplierRelationship, Organisation } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { CreateData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './supplierrelationship-edit.component.html',
  providers: [ContextService]
})
export class SupplierRelationshipEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  partyRelationship: SupplierRelationship;
  internalOrganisation: Organisation;
  organisation: Organisation;
  title: string;

  private fetcher: Fetcher;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<SupplierRelationshipEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.SupplierRelationship({
              object: this.data.id,
              include: {
                InternalOrganisation: x,
                Parties: x
              }
            }),
            pull.Organisation({
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
        this.organisation = loaded.objects.Organisation as Organisation;

        if (isCreate) {

          if (this.organisation === undefined) {
            this.dialogRef.close();
          }

          this.title = 'Add Supplier Relationship';

          this.partyRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
          this.partyRelationship.FromDate = moment.utc();
          this.partyRelationship.Supplier = this.organisation;
          this.partyRelationship.InternalOrganisation = this.internalOrganisation;
        } else {
          this.partyRelationship = loaded.objects.SupplierRelationship as SupplierRelationship;

          if (this.partyRelationship.CanWriteFromDate) {
            this.title = 'Edit Supplier Relationship';
          } else {
            this.title = 'View Supplier Relationship';
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
