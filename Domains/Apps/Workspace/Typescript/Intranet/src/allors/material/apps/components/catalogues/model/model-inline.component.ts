import { Component, EventEmitter, Input, OnDestroy , OnInit, Output } from '@angular/core';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';
import { Model } from '../../../../../domain';
import { PullRequest, Query } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  selector: 'model-inline',
  templateUrl: './model-inline.component.html',
})
export class InlineModelComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<Model> = new EventEmitter<Model>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public model: Model;

  public m: MetaDomain;

  constructor(private workspaceService: WorkspaceService, private errorService: ErrorService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  ngOnInit(): void {
    this.model = this.scope.session.create('Model') as Model;
  }

  public ngOnDestroy(): void {
    if (!!this.model) {
      this.scope.session.delete(this.model);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
      this.saved.emit(this.model);
      this.model = undefined;
  }
}
