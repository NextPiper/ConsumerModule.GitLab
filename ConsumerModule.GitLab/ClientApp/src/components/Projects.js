import React, { Component } from "react";
import style from "./projects.css";
import ProjectListItem from "./ProjectListItem/ProjectListItem";

export class Projects extends Component {
  static displayName = Projects.name;

  constructor(props) {
    super(props);
    this.state = { projects: [], loading: true };
  }

  componentDidMount() {
    this.fetchProjects();
  }

  renderProjects = () => {
    if (this.state.loading) {
      return <div style={{ textAlign: "center" }}>Loading...</div>;
    } else {
      if (this.state.projects.length === 0) {
        return <div>No Resources found...;(</div>;
      } else {
        return this.state.projects.map((item, i) => (
          <ProjectListItem
            projectId={item.projectId}
            projectName={item.name}
            projectScore={item.averageProjectScore}
            onClick={this.onProjectClicked}
          ></ProjectListItem>
        ));
      }
    }
  };

  async fetchProjects() {
    console.log("fetch projects");
    const response = await fetch("http://localhost:7070/projects");
    if (response) {
      console.log(response);
    }
    const data = await response.json();
    console.log(data);
    this.setState({ projects: data, loading: false });
    console.log("Fetching projects done");
  }

  onProjectClicked = (id) => {
    this.props.history.push(`/app/project/${id}`);
  };

  render() {
    return (
      <div className="projectsContainer">
        <div className="projectsHeader">Projects</div>
        <div className="projectsSeperator"></div>
        <div className="projectsList">{this.renderProjects()}</div>
      </div>
    );
  }
}
