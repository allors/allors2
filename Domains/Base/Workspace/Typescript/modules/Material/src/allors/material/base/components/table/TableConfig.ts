import { Sort } from '@angular/material';

import { Action } from '../../../../angular';

import { Column } from './Column';

export interface TableConfig {
    selection?: boolean;

    columns?: (Partial<Column> | string)[];

    actions?: Action[];

    defaultAction?: Action;

    sort?: Partial<Sort>;
}
