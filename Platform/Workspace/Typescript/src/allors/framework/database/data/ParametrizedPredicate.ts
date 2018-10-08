import { Predicate } from './Predicate';
import { And } from './And';
import { Or } from './Or';
import { Not } from './Not';

export class ParametrizedPredicate {
    parameter: string;
}

export function parametrizedPredicates(predicate: Predicate): ParametrizedPredicate[] {
    const results: ParametrizedPredicate[] = [];

    if (predicate instanceof And || predicate instanceof Or) {
        predicate.operands.forEach((v) => results.push(...parametrizedPredicates(v)));
    }

    if (predicate instanceof Not) {
        results.push(...parametrizedPredicates(predicate));
    }

    if (predicate instanceof ParametrizedPredicate) {
        results.push(predicate);
    }

    return results;
}
