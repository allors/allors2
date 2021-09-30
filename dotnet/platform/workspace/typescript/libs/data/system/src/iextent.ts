import { Extent } from './Extent';
import { Union } from './Union';
import { Intersect } from './Intersect';
import { Except } from './Except';

export type IExtent = Extent | Union | Intersect | Except;
