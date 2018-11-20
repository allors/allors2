import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Locale, Organisation } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { MetaDomain, PullFactory } from '../../../../../meta';

import { Loaded, SessionService, WorkspaceService, ErrorService } from '../../../../../angular';

@Component({
  templateUrl: './organisation-overview.component.html',
  providers: [SessionService]
})
export class OrganisationOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;

  public organisation: Organisation;
  public locales: Locale[];

  private subscription: Subscription;

  constructor(
    @Self() private sessionService: SessionService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Organisation Overview';
    this.titleService.setTitle(this.title);

    this.m = this.sessionService.m;
  }

  public ngOnInit(): void {


    const { x, pull } = this.sessionService;

    this.subscription = this.route.url
      .pipe(
        switchMap((url) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const m: MetaDomain = this.m;

          const pulls = [
            pull.Organisation({
              object: id,
              include: {
                Owner: x,
                Employees: x,
              }
            })
          ];

          this.sessionService.session.reset();

          return this.sessionService
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
