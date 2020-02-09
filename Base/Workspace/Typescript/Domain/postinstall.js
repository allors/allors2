#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst)
    .then(() => console.log(basename + ' linked') )
    .catch((e) =>  e.errno && e.errno != -4075 ? console.log(e) : console.log('already linked'))
}

link ('../../../../Core/Workspace/Typescript/Domain/src/allors/domain/core', 'src/allors/domain');

link ('../../../../Platform/Workspace/Typescript/framework', 'src/allors');
