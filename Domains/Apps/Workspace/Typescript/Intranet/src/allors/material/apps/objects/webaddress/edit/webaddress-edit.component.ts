import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Enumeration, TelecommunicationsNumber, ElectronicAddress } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { EditData, CreateData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './webaddress-edit.component.html',
  providers: [ContextService]
})
export class WebAddressEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  contactMechanism: ElectronicAddress;
  contactMechanismTypes: Enumeration[];
  title: string;

  private subscription: Subscription;


  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<WebAddressEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          const pulls = [
            pull.ContactMechanism({
              object: this.data.id,
            }),
            pull.ContactMechanismType({
              predicate: new Equals({ propertyType: m.ContactMechanismType.IsActive, value: true }),
              sort: new Sort(this.m.ContactMechanismType.Name)
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

        this.contactMechanismTypes = loaded.collections.ContactMechanismTypes as Enumeration[];

        if (isCreate) {
          this.title = 'Add Web Address';

          this.contactMechanism = this.allors.context.create('WebAddress') as ElectronicAddress;
        } else {
          this.contactMechanism = loaded.objects.ContactMechanism as ElectronicAddress;

          if (this.contactMechanism.CanWriteElectronicAddressString) {
            this.title = 'Edit Web Address';
          } else {
            this.title = 'View Web Address';
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
          id: this.contactMechanism.id,
          objectType: this.contactMechanism.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }}
