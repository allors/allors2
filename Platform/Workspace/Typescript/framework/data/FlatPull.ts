import { Extent } from './Extent';
import { ISessionObject } from '../workspace';
import { Result } from './Result';
import { Fetch } from './Fetch';
import { Predicate } from './Predicate';
import { Sort } from './Sort';
import { Tree } from './Tree';
import { ParameterTypes } from '../workspace/Types';

export interface FlatPull {
    extentRef?: string;

    extent?: Extent;

    predicate?: Predicate;

    sort?: Sort | Sort[];

    object?: ISessionObject | string;

    results?: Result[];

    fetchRef?: string;

    fetch?: Fetch | any;

    include?: Tree | any;

    parameters?: { [id: string]: ParameterTypes };

    name?: string;

    skip?: number;

    take?: number;
}
