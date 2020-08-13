import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, Loaded } from '@allors/angular/services/core';
import { Organisation } from '@allors/domain/generated';
import { assert } from '@allors/meta/system';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './organisation-overview.component.html',
  providers: [ContextService]
})
export class OrganisationOverviewComponent extends TestScope implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: Meta;

  public organisation: Organisation;
  public locales: Locale[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private metaService: MetaService,
    private titleService: Title,
    private route: ActivatedRoute) {
    super();

    this.title = 'Organisation Overview';
    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {


    const { x, pull } = this.metaService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url) => {

          const id = this.route.snapshot.paramMap.get('id');
          assert(id);

          const pulls = [
            pull.Organisation({
              object: id,
              include: {
                Owner: x,
                Employees: x,
              }
            })
          ];

          this.allors.context.reset();

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.Organisation as Organisation;
      });
  }

  public ngAfterViewInit(): void {
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }
}
