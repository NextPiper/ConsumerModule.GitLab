import React from "react";
import FileListItem from "./FileListItem/FileListItem";

import style from "./projects.css";
import style2 from "./project.css";

class Commit extends React.Component {
  constructor(props) {
    super(props);
    this.state = { commit: null, isLoading: true };

    console.log(
      `ProjectId: ${this.props.match.params.id}, commitId: ${this.props.match.params.commitId}`
    );
  }

  componentDidMount() {
    this.fetchCommit();
  }

  async fetchCommit() {
    console.log("fetch project withId: " + this.props.match.params.id);
    const response = await fetch(
      `projects/${this.props.match.params.id}/${this.props.match.params.commitId}`
    );
    const data = await response.json();
    console.log(data);
    this.setState({ commit: data, isLoading: false });
    console.log("Fetching projects done");
  }

  renderContent() {
    if (this.state.isLoading) {
      return <div>Is Loading...</div>;
    } else {
      if (this.state.commit === null) {
        return <div>No resources found... Try again</div>;
      } else {
        return (
          <React.Fragment>
            <div className="projectHeader">
              Commit: {this.state.commit.checkout_sha.substring(0, 7)} - UserId:{" "}
              {this.state.commit.user_id} - AvgScore:{" "}
              {Math.round(
                (this.state.commit.average_Commit_Score + Number.EPSILON) * 100
              ) / 100}{" "}
              - Files: {this.state.commit.files.length}
            </div>
            <div className="projectsSeperator"></div>
            <div className="commitList">{this.renderFileList()}</div>
          </React.Fragment>
        );
      }
    }
  }

  renderFileList() {
    return this.state.commit.fileDataScores.map((item, i) => (
      <FileListItem
        key={i}
        fileName={item.fileName}
        ref={item.ref}
        baseScore={item.baseScore}
        accumulatedCodeScore={item.accumulatedCodeScore}
        detailedScoreDict={item.detailedScoreDict}
      ></FileListItem>
    ));
  }

  render() {
    return <div className="projectsContainer">{this.renderContent()}</div>;
  }
}

export default Commit;
