import React from "react";
import CommitListItem from "./CommitListItem/CommitListItem";

import style from "./project.css";
import style2 from "./projects.css";

class Project extends React.Component {
  constructor(props) {
    super(props);
    this.state = { project: null, isLoading: true };
  }

  componentDidMount() {
    this.fetchProject();
  }

  async fetchProject() {
    console.log("fetch project withId: " + this.props.match.params.id);
    const response = await fetch(
      `http://localhost:7070/projects/${this.props.match.params.id}`
    );
    const data = await response.json();
    console.log(data);
    this.setState({ project: data, isLoading: false });
    console.log("Fetching projects done");
  }

  onClick = (id) => {
    this.props.history.push(
      `/app/project/${this.props.match.params.id}/commit/${id}`
    );
  };

  renderContent() {
    if (this.state.isLoading) {
      return <div>Loading...</div>;
    } else {
      if (this.state.project === null) {
        return <div>No resources found... Try again</div>;
      } else {
        return (
          <React.Fragment>
            <div className="projectHeader">
              {this.state.project.project_name} -{" "}
              {Math.round(
                (this.state.project.averageProjectScore + Number.EPSILON) * 100
              ) / 100}{" "}
              avg score
            </div>
            <div className="projectsSeperator"></div>
            <div className="commitList">{this.renderCommitList()}</div>
          </React.Fragment>
        );
      }
    }
  }

  renderCommitList() {
    return this.state.project.commits.map((item, i) => (
      <CommitListItem
        key={i}
        onClick={this.onClick}
        id={item.id}
        userId={item.user_id}
        avgCodeScore={item.average_Commit_Score}
        numberOfFiles={item.files.length}
        sha={item.checkout_sha}
      ></CommitListItem>
    ));
  }

  render() {
    return <div className="projectsContainer">{this.renderContent()}</div>;
  }
}

export default Project;
