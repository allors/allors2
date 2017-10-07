import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, EventEmitter, Output , ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../../domain';
import { TelecommunicationsNumber, ContactMechanismPurpose, Enumeration } from '../../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../../../angular';

@Component({
  selector: 'party-contactmechanism-telecommunicationsnumber',
  template:
  `
  <a-mat-input  [object]="telecommunicationsNumber" [roleType]="m.TelecommunicationsNumber.CountryCode"></a-mat-input>
  <a-mat-input  [object]="telecommunicationsNumber" [roleType]="m.TelecommunicationsNumber.AreaCode"></a-mat-input>
  <a-mat-input  [object]="telecommunicationsNumber" [roleType]="m.TelecommunicationsNumber.ContactNumber"></a-mat-input>

  <button mat-button color="primary" type="button" (click)="save()">Save</button>
  <button mat-button color="secondary" type="button"(click)="cancel()">Cancel</button>
`,
})
export class PartyContactMechanismInlineTelecommunicationsNumberComponent implements OnInit {

  private scope: Scope;

  @Output()
  saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  cancelled: EventEmitter<any> = new EventEmitter();

  telecommunicationsNumber: TelecommunicationsNumber;

  m: MetaDomain;

  contactMechanismPurposes: Enumeration[];

  constructor(private allors: AllorsService, private errorService: ErrorService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.scope
      .load('Pull', new PullRequest({}))
      .subscribe((loaded: Loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];

        this.telecommunicationsNumber = this.scope.session.create('TelecommunicationsNumber') as TelecommunicationsNumber;
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
        this.saved.emit(this.telecommunicationsNumber.id);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }
}
