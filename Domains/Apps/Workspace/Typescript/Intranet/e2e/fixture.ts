import Axios from "axios";

import { Person } from "../src/allors/domain";
import { constructorByName } from "../src/allors/domain";
import { MetaPopulation, ObjectType, ObjectTyped, PullRequest, Query, Session, Workspace } from "../src/allors/framework";
import { data, MetaDomain } from "../src/allors/meta";
import { Database, Loaded, Scope } from "../src/allors/promise";
import { AxiosHttp } from "../src/allors/promise/base/http/AxiosHttp";

import { LoginPage } from "./pages/login.po";
import { Population } from "./Population";

export class Fixture {

  public objectTypeByName: { [name: string]: ObjectType; };
  public objectTypes: ObjectType[];
  public classes: ObjectType[];

  public m: MetaDomain;

  public scope: Scope;

  private workspace: Workspace;

  private url = "http://localhost:5000/";

  public constructor() {
    const metaPopulation = new MetaPopulation(data);
    this.workspace = new Workspace(metaPopulation, constructorByName);
    this.objectTypeByName = this.workspace.metaPopulation.objectTypeByName;
    this.objectTypes = Object.keys(this.objectTypeByName).map((key) => this.objectTypeByName[key]);
    this.classes = this.objectTypes.filter((objectType) => objectType.isClass);
    this.m = metaPopulation.metaDomain;
  }

  public async setup(): Promise<any> {

    const http = new AxiosHttp(this.url);
    const database = new Database(http);

    await Axios.get(this.url + "Test/Setup");

    const loginPage = new LoginPage();
    await loginPage.login("administrator", "pwd");

    await http.login("TestAuthentication/Token", "administrator");

    this.scope = new Scope(database, this.workspace);
  }

  public async load(classes: ObjectType[] = null): Promise<Population> {

    try {
      const query: Query[] = (classes || this.classes)
      .map((objectType) => new Query(
      {
        name: objectType.name,
        objectType,
      }));

      const loaded = await this.scope.load("Pull", new PullRequest({ query }));

      const objectsByObjectType: Map<ObjectType, any[]> = new Map();

      Object.keys(loaded.collections).forEach((key) => {
        const objectType = this.objectTypeByName[key];
        objectsByObjectType.set(objectType, loaded.collections[key]);
      });

      const population = new Population(objectsByObjectType);
      return population;
  } catch (e) {
    console.log(e);
    throw e;
  }
}
}
