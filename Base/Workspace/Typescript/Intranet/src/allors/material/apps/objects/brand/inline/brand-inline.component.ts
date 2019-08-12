import { Component, EventEmitter, Input, OnDestroy , OnInit, Output } from '@angular/core';

import { ContextService, WorkspaceService, MetaService } from '../../../../../angular';
import { Brand } from '../../../../../domain';
import { Meta } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'brand-inline',
  templateUrl: './brand-inline.component.html',
})
export class InlineBrandComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<Brand> = new EventEmitter<Brand>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  public brand: Brand;

  public m: Meta;

  constructor(
    private allors: ContextService,
    metaService: MetaService
    ) {

    this.m = metaService.m;
  }

  ngOnInit(): void {
    this.brand = this.allors.context.create('Brand') as Brand;
  }

  public ngOnDestroy(): void {
    if (!!this.brand) {
      this.allors.context.delete(this.brand);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
      this.saved.emit(this.brand);
      this.brand = undefined;
  }
}
