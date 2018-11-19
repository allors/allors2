import { MethodType } from '../../../../../allors/framework';

import { Column } from './Column';
import { Action } from '../../actions';

export interface TableConfig {
    selection?: boolean;

    columns?: (Partial<Column> | string)[];

    actions?: Action[];
}
