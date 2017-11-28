import { SessionObject } from "@allors/framework";
import { Version } from './Version.g';
import { BudgetState } from './BudgetState.g';
import { BudgetRevision } from './BudgetRevision.g';
import { BudgetReview } from './BudgetReview.g';
import { BudgetItem } from './BudgetItem.g';
export interface BudgetVersion extends SessionObject, Version {
    BudgetState: BudgetState;
    FromDate: Date;
    ThroughDate: Date;
    Comment: string;
    Description: string;
    BudgetRevisions: BudgetRevision[];
    AddBudgetRevision(value: BudgetRevision): any;
    RemoveBudgetRevision(value: BudgetRevision): any;
    BudgetNumber: string;
    BudgetReviews: BudgetReview[];
    AddBudgetReview(value: BudgetReview): any;
    RemoveBudgetReview(value: BudgetReview): any;
    BudgetItems: BudgetItem[];
    AddBudgetItem(value: BudgetItem): any;
    RemoveBudgetItem(value: BudgetItem): any;
}
