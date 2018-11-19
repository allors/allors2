import { AfterViewInit, Component, OnDestroy, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { Subscription } from 'rxjs';

import { Organisation, Deletable } from '../../../../domain';
import { MetaDomain } from '../../../../meta';
import { PullRequest, SessionObject } from '../../../../framework';
import { Loaded, Scope, WorkspaceService, Allors, x, NavigationService, Invoked, ErrorService } from '../../../../angular';
import { Table, AllorsMaterialDialogService, ActionTarget } from '../../../../material';

interface Row extends ActionTarget {
  object: Organisation;
  name: string;
  owner: string;
}

@Component({
  templateUrl: './organisations.component.html',
  providers: [Allors]
})
export class OrganisationsComponent implements AfterViewInit, OnDestroy {

  title: string;

  m: MetaDomain;

  table: Table<Row>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    @Self() public allors: Allors,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private snackBar: MatSnackBar,
    private workspaceService: WorkspaceService,
    private titleService: Title,
    private router: Router) {

    this.title = 'Organisations';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.table = new Table({
      selection: true,
      columns: ['name', 'owner'],
      actions: [
        {
          name: () => 'Details',
          handler: (target: ActionTarget) => {
            this.navigation.overview(target.object);
          }
        },
        {
          method: this.m.Deletable.Delete,
          handler: (target: ActionTarget) => this.delete(target)
        }
      ]
    });
  }

  public ngAfterViewInit(): void {
    this.search();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public search(): void {

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { m, pull } = this.allors;

    const pulls = [

      pull.Organisation({
        include: {
          Owner: x,
          Employees: x,
        }
      })
    ];

    this.scope.session.reset();

    this.subscription = this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded: Loaded) => {
        const organisations = loaded.collections.Organisations as Organisation[];
        this.table.data = organisations.map((v) => {
          return {
            object: v,
            name: v.Name,
            owner: v.Owner && v.Owner.UserName
          };
        });
      },
        (error: any) => {
          alert(error);
          this.goBack();
        });
  }

  public goBack(): void {
    this.router.navigate(['/']);
  }

  public refresh() {
    this.search();
  }

  public delete(target: ActionTarget | ActionTarget[]): void {

    const { scope } = this.allors;

    const objects: Deletable[] = (target instanceof Array ? target.map(v => v.object) : [target.object]) as Deletable[];
    const methods = objects.filter((v) => v.CanExecuteDelete).map((v) => v.Delete);

    if (methods.length > 0) {
      this.dialogService
        .confirm(
          methods.length === 1 ?
            { message: 'Are you sure you want to delete this organisation?' } :
            { message: 'Are you sure you want to delete these organisations?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope.invoke(methods)
              .subscribe((invoked: Invoked) => {
                this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
                this.refresh();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          }
        });
    }
  }
}
