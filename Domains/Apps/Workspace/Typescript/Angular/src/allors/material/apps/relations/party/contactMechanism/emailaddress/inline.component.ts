import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, EventEmitter, Output , ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../../domain';
import { EmailAddress } from '../../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../../../angular';

@Component({
  selector: 'party-contactmechanism-emailAddress',
  template:
  `
  <a-md-input [object]="emailAddress" [roleType]="m.EmailAddress.ElectronicAddressString" label="Email address"></a-md-input>

  <button md-button color="primary" type="button" (click)="save()">Save</button>
  <button md-button color="secondary" type="button"(click)="cancel()">Cancel</button>
`,
})
export class PartyContactMechanismInlineEmailAddressComponent implements OnInit {

  private scope: Scope;

  @Output()
  saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  cancelled: EventEmitter<any> = new EventEmitter();

  emailAddress: EmailAddress;

  m: MetaDomain;

  constructor(private allors: AllorsService, private errorService: ErrorService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.scope
      .load('Pull', new PullRequest({}))
      .subscribe((loaded: Loaded) => {
        this.emailAddress = this.scope.session.create('EmailAddress') as EmailAddress;
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
        this.saved.emit(this.emailAddress.id);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }
}
