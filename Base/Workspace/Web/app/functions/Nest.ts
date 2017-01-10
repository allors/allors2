namespace Allors.Domain.Custom
{
    export function nest(collection: any, iteratees: any) {
        if (!iteratees.length) {
            return collection;
        }

        const first = iteratees[0];
        var rest = iteratees.slice(1);

        const sorted = _.sortBy(collection, first);
        const group = _.groupBy(sorted, first);

        return _.mapValues(group, value => nest(value, rest));
    };
}
