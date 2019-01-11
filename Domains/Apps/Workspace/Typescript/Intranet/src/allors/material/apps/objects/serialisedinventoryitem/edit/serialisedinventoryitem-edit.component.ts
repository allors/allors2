import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { SerialisedInventoryItem, SerialisedInventoryItemState } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { CreateData, EditData, ObjectData } from 'src/allors/material/base/services/object';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './serialisedinventoryitem-edit.component.html',
  providers: [ContextService]
})
export class SerialisedInventoryItemEditComponent implements OnInit, OnDestroy {

  public title: string;
  public subTitle: string;

  public m: Meta;

  public serialisedInventoryItem: SerialisedInventoryItem;

  private subscription: Subscription;
  private fetcher: Fetcher;
  serialisedInventoryItemStates: SerialisedInventoryItemState[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<SerialisedInventoryItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          const pulls = [
            pull.SerialisedInventoryItem(
              {
                object: this.data.id,
                include: {
                  SerialisedInventoryItemState: x,
                  SerialisedItem: x
                }
              }
            ),
            pull.SerialisedInventoryItemState({
              predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name),
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
        this.serialisedInventoryItemStates = loaded.collections.SerialisedInventoryItemStates as SerialisedInventoryItemState[];

        if (isCreate) {
          this.title = 'Add inventory item';

          this.serialisedInventoryItem = this.allors.context.create('SerialisedInventoryItem') as SerialisedInventoryItem;
        } else {
          this.serialisedInventoryItem = loaded.objects.SerialisedInventoryItem as SerialisedInventoryItem;

          if (this.serialisedInventoryItem.CanWriteSerialisedInventoryItemState) {
            this.title = 'Edit inventory item';
          } else {
            this.title = 'View inventory item';
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

    this.allors.context
      .save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.serialisedInventoryItem.id,
          objectType: this.serialisedInventoryItem.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
