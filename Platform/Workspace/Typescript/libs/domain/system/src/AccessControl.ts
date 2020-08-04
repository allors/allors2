export class AccessControl {
  constructor(
    public id: string,
    public version: string,
    public permissionIds: Set<string>
  ) {}
}
