import { Sort } from '@angular/material';

import { Action } from '../../../../angular';

import { Column } from './Column';

export interface TableConfig {

    columns?: (Partial<Column> | string)[];

    selection?: boolean;

    actions?: Action[];

    defaultAction?: Action;

    sort?: Partial<Sort>;

    pageSize?: number;

    pageSizeOptions?: number[];
}
