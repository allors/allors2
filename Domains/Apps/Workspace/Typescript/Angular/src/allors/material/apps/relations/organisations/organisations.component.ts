import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Equals, Like, Contains, TreeNode, Sort, Page } from '../../../../domain';
import { Organisation, OrganisationRole } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../angular';

@Component({
  templateUrl: './organisations.component.html',
})
export class OrganisationsComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: Organisation[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService,
  ) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle('Organisations');
    this.search();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  search(criteria?: string): void {

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m: MetaDomain = this.allors.meta;

    const organisationRolesQuery: Query[] = [new Query(
      {
        name: 'organisationRoles',
        objectType: m.OrganisationRole,
      })];

    this.subscription = this.scope
      .load('Pull', new PullRequest({ query: organisationRolesQuery }))
      .mergeMap((loaded: Loaded) => {
        const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
        const customerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === 'Customer');

        const query: Query[] = [new Query(
          {
            name: 'organisations',
            objectType: m.Organisation,
            predicate: new Contains({roleType: m.Organisation.OrganisationRoles, object: customerRole}),
            include: [
              new TreeNode({
                roleType: m.Organisation.GeneralCorrespondence,
                nodes: [
                  new TreeNode({
                    roleType: m.PostalAddress.PostalBoundary,
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                  }),
                ],
              }),
            ],
          })];

        return this.scope.load('Pull', new PullRequest({ query: query }));
      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.organisations as Organisation[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  delete(organisation: Organisation): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this organisation?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(organisation: Organisation): void {
    this.router.navigate(['/relations/organisations/' + organisation.id + '/overview']);
  }
}
