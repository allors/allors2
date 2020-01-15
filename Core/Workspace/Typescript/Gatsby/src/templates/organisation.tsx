import React from "react"
import { graphql } from 'gatsby'

import { Query } from "../../graphql-types"
import Layout from "../components/layout"

export default class OrganisationPage extends React.Component<{ data: Query }> {
  public render() {

    const { name } = this.props.data.allorsOrganisation;

    return (<Layout>
      <h1>{name}</h1>
    </Layout>
    )
  }
}

export const query = graphql`
  query($slug: String!) {
    allorsOrganisation(slug: { eq: $slug }) {
      slug,
      name
    },
  }
`
