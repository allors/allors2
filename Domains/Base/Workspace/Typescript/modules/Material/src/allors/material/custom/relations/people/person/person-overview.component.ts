import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';
import { Locale, Person } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { MetaDomain, PullFactory } from '../../../../../meta';

@Component({
  templateUrl: './person-overview.component.html',
})
export class PersonOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public person: Person;
  public locales: Locale[];

  public title: string;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Person Overview';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    const x = {};
    const pull = new PullFactory(this.workspaceService.metaPopulation);

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Person({
              object: id,
              include: {
                Photo: x,
              }
            })
          ];

          this.scope.session.reset();

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {
        this.person = loaded.objects.Person as Person;
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
