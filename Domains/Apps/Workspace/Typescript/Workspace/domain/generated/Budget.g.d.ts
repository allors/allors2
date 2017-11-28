import { SessionObject } from "@allors/framework";
import { Period } from './Period.g';
import { Commentable } from './Commentable.g';
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { BudgetState } from './BudgetState.g';
export interface Budget extends SessionObject, Period, Commentable, UniquelyIdentifiable {
    BudgetState: BudgetState;
}
