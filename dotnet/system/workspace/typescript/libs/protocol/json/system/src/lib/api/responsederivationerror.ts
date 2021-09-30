export interface ResponseDerivationError {
  /** Message */
  m: string;

  /** Roles
   * [][AssociationId, RelationTypeId]
   */
  r: number[][];
}
