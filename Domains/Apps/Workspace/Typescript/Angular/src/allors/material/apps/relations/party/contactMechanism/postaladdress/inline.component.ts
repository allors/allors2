import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, EventEmitter, Output , ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../../domain';
import { PostalAddress, PostalBoundary, Country, Enumeration } from '../../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../../../angular';

@Component({
  selector: 'party-contactmechanism-postaladdress',
  template:
  `
  <a-md-input  [object]="postalAddress" [roleType]="m.PostalAddress.Address1" label="Address line 1"></a-md-input>
  <a-md-input  [object]="postalAddress" [roleType]="m.PostalAddress.Address2" label="Address line 2"></a-md-input>
  <a-md-input  [object]="postalAddress" [roleType]="m.PostalAddress.Address3" label="Address line 3"></a-md-input>
  <a-md-input  [object]="postalAddress.PostalBoundary" [roleType]="m.PostalBoundary?.Locality" label="City"></a-md-input>
  <a-md-input  [object]="postalAddress.PostalBoundary" [roleType]="m.PostalBoundary?.PostalCode" label="Postal code"></a-md-input>
  <a-md-select  [object]="postalAddress.PostalBoundary" [roleType]="m.PostalBoundary?.Country" [options]="countries" display="Name"></a-md-select>

  <button md-button color="primary" type="button" (click)="save()">Save</button>
  <button md-button color="secondary" type="button"(click)="cancel()">Cancel</button>
`,
})
export class PartyContactMechanismInlinePostalAddressComponent implements OnInit {

  private scope: Scope;

  @Output()
  saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  cancelled: EventEmitter<any> = new EventEmitter();

  postalAddress: PostalAddress;
  postalBoundary: PostalBoundary;
  countries: Country[];

  m: MetaDomain;

  constructor(private allors: AllorsService, private errorService: ErrorService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    const query: Query[] = [
      new Query(
        {
          name: 'countries',
          objectType: this.m.Country,
        }),
    ];

    this.scope
      .load('Pull', new PullRequest({ query: query }))
      .subscribe((loaded: Loaded) => {
        this.countries = loaded.collections.countries as Country[];
        this.postalAddress = this.scope.session.create('PostalAddress') as PostalAddress;
        this.postalBoundary = this.scope.session.create('PostalBoundary') as PostalBoundary;
        this.postalAddress.PostalBoundary = this.postalBoundary;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  cancel(): void {
    this.cancelled.emit();
  }

  save(): void {
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.postalAddress.id);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }
}
