#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst)
    .then(() => console.log(basename + ' linked') )
    .catch((e) =>  e.errno && e.errno != -4075 ? console.log(e) : console.log('already linked'))
}

// System
link ('../../../Platform/Workspace/Typescript/libs/data/system/src', 'libs/data/system');
link ('../../../Platform/Workspace/Typescript/libs/domain/system/src', 'libs/domain/system');
link ('../../../Platform/Workspace/Typescript/libs/meta/system/src', 'libs/meta/system');
link ('../../../Platform/Workspace/Typescript/libs/protocol/system/src', 'libs/protocol/system');

// Core
link ('../../../Core/Workspace/Typescript/libs/angular/core/src', 'libs/angular/core');
link ('../../../Core/Workspace/Typescript/libs/angular/material/core/src', 'libs/angular/material/core');
link ('../../../Core/Workspace/Typescript/libs/domain/core/src', 'libs/domain/core');
link ('../../../Core/Workspace/Typescript/libs/meta/core/src', 'libs/meta/core');
