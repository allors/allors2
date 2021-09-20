export interface AuthenticationTokenResponse {
  /** Authenticated */
  a: boolean;

  /** User id */
  u: number;

  /** Token */
  t: string;
}
