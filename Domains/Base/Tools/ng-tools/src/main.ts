#! /usr/bin/env node
import * as path from 'path';
import { Program } from './Program';

import { menu } from '../../../Workspace/Typescript/modules/Material/src/app/main/main.menu';


//const project = path.resolve("./projects/empty/src/tsconfig.app.json");
const project = path.resolve("../../Workspace/Typescript/modules/Material/src/tsconfig.app.json");

const program = new Program(project);
const { application } = program;

// console.log()
// console.log("Root Components")
// console.log("===============")
// application.directives.filter((v)=>v.isComponent && v.isLocal && v.hasSelector).forEach((v) => console.log(v.name))

// console.log()
// console.log("Selector Components")
// console.log("===================")
// application.directives.filter((v)=>v.isComponent && v.isLocal && !v.hasSelector).forEach((v) => console.log(v.name))

console.log()
console.log("Root Components")
console.log("===============")
menu.forEach((v)=> console.log(v));
