export type PredicateArgs = Pick<Predicate, "dependencies">;

export abstract class Predicate {
  dependencies?: string[];
}
