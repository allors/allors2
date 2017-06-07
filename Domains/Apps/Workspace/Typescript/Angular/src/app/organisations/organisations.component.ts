import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';

import { Scope } from '../../allors/angular/base/Scope';
import { AllorsService } from '../allors.service';

import { Organisation } from '../../allors/domain';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './organisations.component.html'
})
export class OrganisationsComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  data: Organisation[];

  constructor(private titleService: Title,
    private router: Router,
    private loadingService: TdLoadingService,
    private dialogService: TdDialogService,
    private snackBarService: MdSnackBar,
    public media: TdMediaService,
    allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  goBack(route: string): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.titleService.setTitle('Organisations');
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  search(criteria: string) {

    if (!criteria || criteria.length <= 2) {
      this.data = undefined;
      return;
    }

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.scope.session.reset();

    return this.scope
      .load('Organisations', { criteria: criteria })
      .do(() => {
        this.data = this.scope.collections.organisations as Organisation[];
      })
      .subscribe();
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
    this.router.navigate(['/organisations/' + organisation.id + '/overview']);
  }
}
