import { Extent } from './Extent';
import { ISessionObject } from '../../workspace';
import { Result } from './Result';
import { Filter } from './Filter';
import { Fetch } from './Fetch';
import { Path } from './Path';
import { Tree } from './Tree';
import { ObjectType } from '../../meta';
import { Pull } from './Pull';
import { Predicate } from './Predicate';

export interface FlatPull {

    extentRef?: string;

    extent?: Extent;

    predicate?: Predicate;

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
