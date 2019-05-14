#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst, {force: true})
    .then(() => console.log(basename + ' linked') )
    .catch(() =>  console.log(basename + ' already linked'))
}

link ('../../../../Platform/Framework/Typescript/framework', 'src/allors');

link ('../Domain/src/allors/meta/base', 'src/allors/meta');
link ('../Angular/src/allors/angular/base', 'src/allors/angular');
