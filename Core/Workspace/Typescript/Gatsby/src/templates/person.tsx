import React from "react"
import { graphql } from 'gatsby'

import { Query } from "../../graphql-types"
import Layout from "../components/layout"

export default class PersonPage extends React.Component<{ data: Query }> {
  public render() {

    const { firstName } = this.props.data.allorsPerson;

    return (<Layout>
      <h1>{firstName}</h1>
    </Layout>
    )
  }
}

export const query = graphql`
  query($slug: String!) {
    allorsPerson(slug: { eq: $slug }) {
      slug,
      firstName
    },
  }
`
