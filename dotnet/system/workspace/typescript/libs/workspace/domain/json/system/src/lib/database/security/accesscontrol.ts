export class AccessControl {
  constructor(public id: number, public version: number, public permissionIds: Set<number>) {}
}
