import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

export class Field {
  @Input()
  object: ISessionObject;

  @Input()
  roleType: RoleType;

  @Input()
  required: boolean;

  get model(): any {
    if (this.isReady) {
      return this.object[this.roleType.name];
    }
  }

  get name(): string {
    if (this.isReady) {
      return this.roleType.name;
    }
  }

  get canRead(): boolean {
    if (this.isReady) {
      return this.object.canRead(this.roleType.name);
    }
  }

  get canWrite(): boolean {
    if (this.isReady) {
      return this.object.canWrite(this.roleType.name);
    }
  }

  get isRequired(): boolean {
    if (this.required) {
      return this.required;
    }

    if (this.isReady) {
      return this.roleType.isRequired;
    }
  }

  private get isReady(): boolean {
    return !!(this.object && this.roleType);
  }
}
