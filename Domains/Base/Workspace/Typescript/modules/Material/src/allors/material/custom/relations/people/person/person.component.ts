import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Locale, Person } from '../../../../../domain';
import { PullRequest, Pull } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';
import { PullFactory } from '../../../../../meta/generated/pull.g';

@Component({
  templateUrl: './person.component.html',
})
export class PersonComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;
  public locales: Locale[];
  public person: Person;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Person';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    const pull = new PullFactory(this.workspaceService.metaPopulation);

    this.subscription = this.route.url
      .pipe(
        switchMap(() => {
          const x = {};
          const id: string = this.route.snapshot.paramMap.get('id');
          const pulls: Pull[] = [
            pull.Person({
              object: id,
              include: {
                Photo: x,
                Pictures: x,
              }
            }),
            pull.Locale()
          ];
          this.scope.session.reset();
          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {

        this.person = loaded.objects.Person as Person || this.scope.session.create('Person') as Person;
        this.locales = loaded.collections.Locales as Locale[];
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

  public save(): void {

    this.scope
      .save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
