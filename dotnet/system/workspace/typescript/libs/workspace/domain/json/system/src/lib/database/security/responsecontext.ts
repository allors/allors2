import { Numbers } from '../../collections/Numbers';
import { AccessControl } from './AccessControl';
import { Permission } from './Permission';

export class ResponseContext {
  constructor(private readonly accessControlById: Map<number, AccessControl>, private readonly permissionById: Map<number, Permission>) {
    this.missingAccessControlIds = new Set();
    this.missingPermissionIds = new Set();
  }

  missingAccessControlIds: Set<number>;

  missingPermissionIds: Set<number>;

  checkForMissingAccessControls(value: Numbers): Numbers {
    return value?.filter((v) => !this.accessControlById.has(v));
  }

  checkForMissingPermissions(value: Numbers): Numbers {
    return value?.filter((v) => !this.permissionById.has(v));
  }
}
