import { Meta } from "@allors/meta/generated";
import { roles } from '@allors/gatsby/source/core';

export function schema(m: Meta){

  m.Organisation.gatsbyRoleTypes = [m.Organisation.Name, m.Organisation.Owner];
  m.Organisation.gatsbyProperties = [{ name: roles.Slug, type: 'String' }];
  
  m.Person.gatsbyRoleTypes = [m.Person.FirstName, m.Person.Photo];
  m.Person.gatsbyProperties = [{ name: roles.Slug, type: 'String' }];
  
  m.Media.gatsbyRoleTypes = [m.Media.UniqueId, m.Media.Revision, m.Media.FileName];
  
  m.gatsbyDerive();
}
