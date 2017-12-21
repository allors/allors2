#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst)
    .then(() => console.log(basename + ' linked') )
    .catch(() =>  console.log(basename + ' already linked'))
}

link ('../../../../../Platform/Workspace/Typescript/src/allors/framework', 'src/allors');

link ('../../../../Base/Workspace/Typescript/Angular/src/allors/angular/base', 'src/allors/angular');
link ('../../../../Base/Workspace/Typescript/Material/src/allors/material/base', 'src/allors/material');
link ('../../../../Base/Workspace/Typescript/Covalent/src/allors/covalent/base', 'src/allors/covalent');
link ('../../../../Base/Workspace/Typescript/Promise/src/allors/promise/base', 'src/allors/promise');
link ('../Domain/src/allors/domain/apps', 'src/allors/domain');