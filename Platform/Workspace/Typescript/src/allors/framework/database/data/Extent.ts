import { Filter } from './Filter';
import { Union } from './Union';
import { Intersect } from './Intersect';
import { Except } from './Except';

export type Extent = Filter | Union | Intersect | Except;
