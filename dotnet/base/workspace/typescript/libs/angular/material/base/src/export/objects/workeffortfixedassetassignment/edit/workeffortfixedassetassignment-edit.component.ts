import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { Enumeration, Party, Organisation, SerialisedItem, WorkEffortFixedAssetAssignment, WorkEffort } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId, Filters } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope, SearchFactory } from '@allors/angular/core';


@Component({
  templateUrl: './workeffortfixedassetassignment-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortFixedAssetAssignmentEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  workEffortFixedAssetAssignment: WorkEffortFixedAssetAssignment;
  workEffort: WorkEffort;
  assignment: WorkEffort;
  serialisedItem: SerialisedItem;
  assetAssignmentStatuses: Enumeration[];
  title: string;

  private subscription: Subscription;
  serialisedItems: SerialisedItem[];
  externalCustomer: boolean;

  serialisedItemsFilter: SearchFactory;
  workEfforts: WorkEffort[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<WorkEffortFixedAssetAssignmentEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            pull.WorkEffort({
              sort: new Sort(m.WorkEffort.Name)
            }),
            pull.SerialisedItem({
              object: this.data.associationId,
              sort: new Sort(m.SerialisedItem.Name)
            }),
            pull.AssetAssignmentStatus({
              predicate: new Equals({ propertyType: m.AssetAssignmentStatus.IsActive, value: true }),
              sort: new Sort(m.AssetAssignmentStatus.Name)
            }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.WorkEffortFixedAssetAssignment({
                object: this.data.id,
                include: {
                  Assignment: x,
                  FixedAsset: x,
                  AssetAssignmentStatus: x
                }
              }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.WorkEffort({
                object: this.data.associationId,
              }),
            );
          }

          this.serialisedItemsFilter = Filters.serialisedItemsFilter(m);

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.workEffort = loaded.objects.WorkEffort as WorkEffort;
        this.workEfforts = loaded.collections.WorkEfforts as WorkEffort[];
        this.serialisedItem = loaded.objects.SerialisedItem as SerialisedItem;
        this.assetAssignmentStatuses = loaded.collections.AssetAssignmentStatuses as Enumeration[];

        if (this.serialisedItem === undefined) {
          const b2bCustomer = this.workEffort.Customer as Organisation;
          this.externalCustomer = b2bCustomer === null || !b2bCustomer.IsInternalOrganisation;
  
          if (this.externalCustomer) {
            this.updateSerialisedItems(this.workEffort.Customer);
          }
        }

        if (isCreate) {
          this.title = 'Add Asset Assignment';

          this.workEffortFixedAssetAssignment = this.allors.context.create('WorkEffortFixedAssetAssignment') as WorkEffortFixedAssetAssignment;

          if (this.serialisedItem !== undefined) {
            this.workEffortFixedAssetAssignment.FixedAsset = this.serialisedItem;
          }

          if (this.workEffort !== undefined && this.workEffort.objectType.name === m.WorkTask.name) {
            this.assignment = this.workEffort as WorkEffort;
            this.workEffortFixedAssetAssignment.Assignment = this.assignment;
          }

        } else {
          this.workEffortFixedAssetAssignment = loaded.objects.WorkEffortFixedAssetAssignment as WorkEffortFixedAssetAssignment;

          if (this.workEffortFixedAssetAssignment.CanWriteFromDate) {
            this.title = 'Edit Asset Assignment';
          } else {
            this.title = 'View Asset Assignment';
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
          id: this.workEffortFixedAssetAssignment.id,
          objectType: this.workEffortFixedAssetAssignment.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  private updateSerialisedItems(customer: Party) {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party(
        {
          object: customer,
          fetch: {
            SerialisedItemsWhereOwnedBy: x,
          },
        }
      ),
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        this.serialisedItems = loaded.collections.SerialisedItems as SerialisedItem[];
      });
  }
}
