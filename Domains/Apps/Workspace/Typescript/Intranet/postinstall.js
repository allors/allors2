#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
 
    lnk([src], dst, {force: true})
    .then(() => console.log(src + ' linked to ' + dst) )
    .catch((e) =>  console.log(src + ' already linked ' + dst))
}

link ('../../../../../Platform/Workspace/Typescript/src/allors/framework', 'src/allors');

link ('../../../../Base/Workspace/Typescript/modules/Angular/src/allors/angular/base', 'src/allors/angular');
link ('../../../../Base/Workspace/Typescript/modules/Material/src/allors/material/base', 'src/allors/material');
link ('../../../../Base/Workspace/Typescript/modules/Covalent/src/allors/covalent/base', 'src/allors/covalent');
link ('../../../../Base/Workspace/Typescript/modules/Promise/src/allors/promise/base', 'src/allors/promise');

link ('../Domain/src/allors/domain/apps', 'src/allors/domain');