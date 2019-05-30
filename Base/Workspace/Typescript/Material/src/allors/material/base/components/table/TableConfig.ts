import { Sort } from '@angular/material/sort';

import { Action } from '../../../../angular';

import { Column } from './Column';

export interface TableConfig {
    columns?: (Partial<Column> | string)[];

    selection?: boolean;

    actions?: Action[];

    defaultAction?: Action;

    autoSort?: boolean;

    initialSort?: Partial<Sort> | string;

    pageSize?: number;

    pageSizeOptions?: number[];

    autoFilter?: boolean;
}
