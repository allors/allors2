#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst, {force: true})
    .then(() => console.log(basename + ' linked') )
    .catch((e) =>  e.errno && e.errno != -4075 ? console.log(e) : console.log('already linked'))
}

link ('../../../../Platform/Workspace/Typescript/framework', 'src/allors');

link ('../../../../Core/Workspace/Typescript/Domain/src/allors/meta/core', 'src/allors/meta');
link ('../../../../Core/Workspace/Typescript/Domain/src/allors/domain/core', 'src/allors/domain');
link ('../../../../Core/Workspace/Typescript/Angular/src/allors/angular/core', 'src/allors/angular');
link ('../../../../Core/Workspace/Typescript/Material/src/allors/material/core', 'src/allors/material');

link ('../Domain/src/allors/domain/base', 'src/allors/domain');
