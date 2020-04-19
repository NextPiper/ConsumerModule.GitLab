import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Projects } from "./components/Projects";
import Project from "./components/Project";
import Commit from "./components/Commit";

import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/app" component={Projects} />
        <Route exact path="/app/project/:id" component={Project} />
        <Route
          exact
          path="/app/project/:id/commit/:commitId"
          component={Commit}
        />
      </Layout>
    );
  }
}
