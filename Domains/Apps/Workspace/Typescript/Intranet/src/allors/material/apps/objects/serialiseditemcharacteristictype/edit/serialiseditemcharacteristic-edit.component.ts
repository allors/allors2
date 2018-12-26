import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { IUnitOfMeasure, Locale, SerialisedItemCharacteristicType, Singleton, TimeFrequency, UnitOfMeasure } from '../../../../../domain';
import { Fetch, PullRequest, Sort, TreeNode, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { CreateData, EditData, ObjectData } from 'src/allors/material/base/services/object';
import { StateService } from '../../../services/state';

@Component({
  templateUrl: './serialiseditemcharacteristic-edit.component.html',
  providers: [ContextService]
})
export class SerialisedItemCharacteristicEditComponent implements OnInit, OnDestroy {

  public title = 'Product Characteristic';
  public subTitle: string;

  public m: Meta;

  public productCharacteristic: SerialisedItemCharacteristicType;

  public singleton: Singleton;
  public uoms: IUnitOfMeasure[];
  public timeFrequencies: TimeFrequency[];
  public allUoms: IUnitOfMeasure[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<SerialisedItemCharacteristicEditComponent>,
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

          const create = (this.data as EditData).id === undefined;
          const { id, objectType, associationRoleType } = this.data;

          const pulls = [
            pull.SerialisedItemCharacteristicType(
              {
                object: id,
                include: {
                  LocalisedNames: {
                    Locale: x,
                  }
                }
              }
            ),
            pull.Singleton({
              include: {
                AdditionalLocales: {
                  Language: x,
                }
              }
            }),
            pull.UnitOfMeasure({
              predicate: new Equals({ propertyType: m.UnitOfMeasure.IsActive, value: true }),
              sort: new Sort(m.UnitOfMeasure.Name),
            }),
            pull.TimeFrequency({
              predicate: new Equals({ propertyType: m.TimeFrequency.IsActive, value: true }),
              sort: new Sort(m.TimeFrequency.Name),
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, create }))
            );
        })
      )
      .subscribe(({ loaded, create }) => {

        this.productCharacteristic = loaded.objects.SerialisedItemCharacteristicType as SerialisedItemCharacteristicType;
        if (create) {
          this.title = 'Add Product Characteristic';
          this.productCharacteristic = this.allors.context.create('SerialisedItemCharacteristicType') as SerialisedItemCharacteristicType;
        } else {
          this.title = 'Edit Product Characteristic';
        }

        this.singleton = loaded.collections.Singletons[0] as Singleton;
        this.uoms = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        this.timeFrequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        this.allUoms = this.uoms.concat(this.timeFrequencies).sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
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
      .subscribe((saved: Saved) => {
        const data: ObjectData = {
          id: this.productCharacteristic.id,
          objectType: this.productCharacteristic.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
