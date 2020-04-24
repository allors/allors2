export class SecurityRequest {
  public accessControls: string[];

  public permissions: string[];

  constructor(fields?: Partial<SecurityRequest>) {
    Object.assign(this, fields);
  }
}
