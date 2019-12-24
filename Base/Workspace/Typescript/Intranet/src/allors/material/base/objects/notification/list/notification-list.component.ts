import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, UserId, TestScope } from '../../../../../angular';
import { TableRow, Table, DeleteService, MethodService } from '../../../..';
import { Notification } from '../../../../../domain';
import { ObjectService } from '../../../../core/services/object';

interface Row extends TableRow {
  object: Notification;
  title: string;
  description: string;
  dateCreated: string;
}

@Component({
  templateUrl: './notification-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class NotificationListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Notifications';

  table: Table<Row>;

  confirm: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public methodService: MethodService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private userId: UserId,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    const { m } = this.metaService;

    this.confirm = methodService.create(allors.context, m.Notification.Confirm, { name: 'Confirm' });

    this.table = new Table({
      selection: true,
      columns: [
        'title',
        'description',
        'dateCreated'
      ],
      actions: [
        this.confirm
      ],
      pageSize: 50,
      initialSort: 'dateCreated'
    });
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, [, , , , ]),
        switchMap(([, , ]) => {

          const pulls = [
            pull.Person({
              object: this.userId.value,
              fetch: {
                NotificationList: {
                  UnconfirmedNotifications: x
                }
              }
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        const notifications = loaded.collections.UnconfirmedNotifications as Notification[];
        this.table.total = loaded.values.Notifications_total;
        this.table.data = notifications.map((v) => {
          return {
            object: v,
            title: v.Title,
            description: v.Description,
            dateCreated: moment(v.DateCreated).fromNow()
          } as Row;
        });
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
