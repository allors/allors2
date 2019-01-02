import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { Subscription, BehaviorSubject } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';

import { ErrorService, ContextService, NavigationService, PanelService, RefreshService, MetaService, Saved } from '../../../../../../angular';
import { Enumeration, InternalOrganisation, Locale, Organisation, SerialisedItem, SerialisedItemState, Ownership, Part } from '../../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-overview-detail',
  templateUrl: './serialiseditem-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class SerialisedItemOverviewDetailComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  serialisedItem: SerialisedItem;

  internalOrganisation: InternalOrganisation;
  locales: Locale[];
  serialisedItemStates: Enumeration[];
  ownerships: Enumeration[];
  parts: Part[];
  activeSuppliers: Organisation[];
  part: Part;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public location: Location,
    public stateService: StateService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Serialised Asset data';
    panel.icon = 'business';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.SerialisedItem.name}`;

    panel.onPull = (pulls) => {

      this.serialisedItem = undefined;

      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.SerialisedItem({
            name: pullName,
            object: id,
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.serialisedItem = loaded.objects[pullName] as SerialisedItem;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.serialisedItem = undefined;

          const { m, pull, x } = this.metaService;
          const fetcher = new Fetcher(this.stateService, this.metaService.pull);
          const id = this.panel.manager.id;

          const pulls = [
            pull.SerialisedItem({
              object: id,
              include: {
                SerialisedItemState: x,
                Ownership: x,
                OwnedBy: x,
                RentedBy: x,
              }
            }),
            fetcher.internalOrganisation,
            fetcher.locales,
            pull.SerialisedItem({
              object: id,
              fetch: {
                PartWhereSerialisedItem: x
              }
            }),
            pull.Part(),
            pull.SerialisedItemState({
              predicate: new Equals({ propertyType: m.SerialisedItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedItemState.Name),
            }),
            pull.Ownership({
              predicate: new Equals({ propertyType: m.Ownership.IsActive, value: true }),
              sort: new Sort(m.Ownership.Name),
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.serialisedItem = loaded.objects.Person as SerialisedItem;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.serialisedItemStates = loaded.collections.SerialisedItemStates as Enumeration[];
        this.ownerships = loaded.collections.Ownerships as Enumeration[];
        this.part = loaded.objects.Part as Part;
        this.parts = loaded.collections.Parts as Part[];

        this.activeSuppliers = this.internalOrganisation.ActiveSuppliers as Organisation[];
        this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
      }, this.errorService.handler);

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.onSave();

    this.allors.context.save()
      .subscribe(() => {
        this.location.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {
    const { context } = this.allors;

    context
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  private onSave() {
    this.part.AddSerialisedItem(this.serialisedItem);
  }
}
