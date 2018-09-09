import { Extent } from './Extent';
import { ISessionObject } from '../../workspace';
import { Result } from './result';
import { Filter } from './Filter';
import { Fetch } from './Fetch';
import { Path, path } from './Path';
import { Tree, tree } from './Tree';
import { ObjectType } from '../../meta';
import { Pull } from './Pull';
import { Predicate } from './Predicate';

export interface FlatPull {

    extentRef?: string;

    extent?: Extent;

    predicate?: Predicate;

    object?: ISessionObject | string;

    results?: Result[];

    fetchRef?: string

    fetch?: Fetch;

    name?: string;

    skip?: number;

    take?: number;

    path?: Path | any;

    include?: Tree | any;
}

export function pull(objectType: ObjectType, flat: FlatPull): Pull {

    if (!flat) {
        return new Pull({
            extent: new Filter({ objectType: objectType })
        })
    }

    var pull = new Pull({
        extentRef: flat.extentRef,
        extent: flat.extent,
        object: flat.object,
        results: flat.results,
    });

    if (flat.predicate) {
        if (pull.extent || pull.extentRef) {
            throw new Error("predicate conflicts with extent/extentRef")
        }

        pull.extent = new Filter({ objectType: objectType, predicate: flat.predicate })
    }

    if (flat.fetchRef || flat.fetch || flat.name || flat.skip || flat.take || flat.path || flat.include) {
        var result = new Result({
            fetchRef: flat.fetchRef,
            fetch: flat.fetch,
            name: flat.name,
            skip: flat.skip,
            take: flat.take
        })


        if (flat.path || flat.include) {
            if (!result.fetch) {
                result.fetch = new Fetch();
            }

            var fetch = result.fetch;

            if (flat.path) {
                fetch.path = flat.path instanceof Path ? flat.path : path(objectType, flat.path);
                if (flat.include) {
                    if (!(flat.include instanceof Tree)) {
                        throw new Error("literal include conflicts with path")
                    }

                    fetch.include = flat.include;
                }
            } else {
                fetch.include = flat.include instanceof Tree ? flat.include : tree(objectType, flat.include);
            }
        }

        pull.results = pull.results || [];
        pull.results.push(result);
    }

    return pull;
}
