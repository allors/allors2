import { Component, EventEmitter, Input, OnDestroy , OnInit, Output } from '@angular/core';

import { ContextService, WorkspaceService, MetaService } from '../../../../../angular';
import { Model } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'model-inline',
  templateUrl: './model-inline.component.html',
})
export class InlineModelComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<Model> = new EventEmitter<Model>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  public model: Model;

  public m: MetaDomain;

  constructor(
    private allors: ContextService,
    private metaService: MetaService,
    ) {

    this.m = this.metaService.m;
  }

  ngOnInit(): void {
    this.model = this.allors.context.create('Model') as Model;
  }

  public ngOnDestroy(): void {
    if (!!this.model) {
      this.allors.context.delete(this.model);
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
