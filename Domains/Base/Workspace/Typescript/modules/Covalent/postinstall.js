#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst)
    .then(() => console.log(basename + ' linked') )
    .catch(() =>  console.log(basename + ' already linked'))
}

link ('../../../../../../Platform/Workspace/Typescript/src/allors/framework', 'src/allors');
link ('../Angular/src/allors/angular/base', 'src/allors/angular');
link ('../Material/src/allors/material/base', 'src/allors/material');