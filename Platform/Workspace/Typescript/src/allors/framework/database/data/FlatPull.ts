import { Extent } from './Extent';
import { ISessionObject } from '../../workspace';
import { Result } from './Result';
import { Fetch } from './Fetch';
import { Path } from './Path';
import { Tree } from './Tree';
import { Predicate } from './Predicate';
import { Sort } from './Sort';

export interface FlatPull {

    extentRef?: string;

    extent?: Extent;

    predicate?: Predicate;

    sort?: Sort | Sort[];

    object?: ISessionObject | string;

    results?: Result[];

    fetchRef?: string;

    fetch?: Fetch;

    name?: string;

    skip?: number;

    take?: number;

    path?: Path | any;

    include?: Tree | any;
}
