import { Sort } from '@angular/material/sort';

import { RoleType } from '../../../framework';
import { Sort as AllorsSort } from './../../../framework';

export class Sorter {
    private config: { [index: string]: RoleType | RoleType[] };

    constructor(config: { [index: string]: RoleType | RoleType[] }) {
        this.config = config;
    }

    create(sort: Sort): any {

        if (sort) {
            const descending = sort.direction === 'desc';
            const roleTypeOrRoleTypes = this.config[sort.active];

            if (roleTypeOrRoleTypes instanceof Array) {
                return (roleTypeOrRoleTypes as RoleType[]).map(v => new AllorsSort({ roleType: v as RoleType, descending }));
            } else {
                return new AllorsSort({ roleType: roleTypeOrRoleTypes as RoleType, descending });
            }
        }
    }
}
