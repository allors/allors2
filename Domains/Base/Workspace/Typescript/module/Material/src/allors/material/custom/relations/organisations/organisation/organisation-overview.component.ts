import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Locale, Organisation } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';

import { Loaded, ContextService, WorkspaceService, ErrorService, MetaService } from '../../../../../angular';

@Component({
  templateUrl: './organisation-overview.component.html',
  providers: [ContextService]
})
export class OrganisationOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: Meta;

  public organisation: Organisation;
  public locales: Locale[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private metaService: MetaService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Organisation Overview';
    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {


    const { x, pull } = this.metaService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const m: Meta = this.m;

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
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.Organisation as Organisation;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
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
