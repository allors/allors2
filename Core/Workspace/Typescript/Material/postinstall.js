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

link ('../Domain/src/allors/core/meta', 'src/allors/core');
link ('../Domain/src/allors/core/domain', 'src/allors/core');
link ('../Angular/src/allors/core/angular', 'src/allors/core');
