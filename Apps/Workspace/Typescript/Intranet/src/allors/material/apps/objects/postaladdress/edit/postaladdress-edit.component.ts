import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { PostalAddress, PostalBoundary, Country, Party, PartyContactMechanism } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { SaveService } from '../../../../../../allors/material';

@Component({
  templateUrl: './postaladdress-edit.component.html',
  providers: [ContextService]
})
export class PostalAddressEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  contactMechanism: PostalAddress;
  postalBoundary: PostalBoundary;
  countries: Country[];
  title: string;

  private subscription: Subscription;
  party: Party;
  partyContactMechanism: PartyContactMechanism;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: IObject,
    public dialogRef: MatDialogRef<PostalAddressEditComponent>,
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
        switchMap(([, ]) => {

          const pulls = [
            pull.ContactMechanism({
              object: this.data.id,
              include: {
                PostalAddress_Country: x
              },
            }),
            pull.Country({
              sort: new Sort(m.Country.Name)
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.countries = loaded.collections.Countries as Country[];
        this.contactMechanism = loaded.objects.ContactMechanism as PostalAddress;

        if (this.contactMechanism.CanWriteAddress1) {
          this.title = 'Edit Postal Address';
        } else {
          this.title = 'View Postal Address';
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
          id: this.contactMechanism.id,
          objectType: this.contactMechanism.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
