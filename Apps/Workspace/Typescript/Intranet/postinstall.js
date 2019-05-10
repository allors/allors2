#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst, {force: true})
    .then(() => console.log(basename + ' linked') )
    .catch(() =>  console.log(basename + ' already linked'))
}

link ('../../../../../Platform/Workspace/Typescript/src/allors/framework', 'src/allors');

link ('../../../../Base/Workspace/Typescript/modules/Domain/src/allors/meta/base', 'src/allors/meta');
link ('../../../../Base/Workspace/Typescript/modules/Domain/src/allors/domain/base', 'src/allors/domain');
link ('../../../../Base/Workspace/Typescript/modules/Angular/src/allors/angular/base', 'src/allors/angular');
link ('../../../../Base/Workspace/Typescript/modules/Material/src/allors/material/base', 'src/allors/material');

link ('../Domain/src/allors/domain/apps', 'src/allors/domain');