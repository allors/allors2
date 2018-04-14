import { Component, EventEmitter, Input, OnDestroy , OnInit, Output } from "@angular/core";

import { ErrorService, Loaded, Scope, WorkspaceService } from "../../../../../angular";
import { Brand } from "../../../../../domain";
import { PullRequest, Query } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  selector: "brand-inline",
  templateUrl: "./brand-inline.component.html",
})
export class InlineBrandComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<Brand> = new EventEmitter<Brand>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public brand: Brand;

  public m: MetaDomain;

  constructor(private workspaceService: WorkspaceService, private errorService: ErrorService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  ngOnInit(): void {
    this.brand = this.scope.session.create("Brand") as Brand;
  }

  public ngOnDestroy(): void {
    if (!!this.brand) {
      this.scope.session.delete(this.brand);
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
