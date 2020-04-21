import React from "react";
import CommitListItem from "./CommitListItem/CommitListItem";
import { Chart } from "react-charts";

import style from "./project.css";
import style2 from "./projects.css";

class Project extends React.Component {
  constructor(props) {
    super(props);
    this.state = { project: null, isLoading: true, searchWord: "" };
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

  onInputChange = (event) => {
    this.setState({ searchWord: event.target.value });
  };

  renderMainGraph = () => {
    var bData = [];
    var aData = [];

    for (var i = 0; i < this.state.project.projectHistory.length; i++) {
      var aSum = 0;
      var bSum = 0;
      for (
        var j = 0;
        j < this.state.project.projectHistory[i].fileDataScores.length;
        j++
      ) {
        aSum += this.state.project.projectHistory[i].fileDataScores[j]
          .accumulatedCodeScore;
        bSum += this.state.project.projectHistory[i].fileDataScores[j]
          .baseScore;
      }
      var current_datetime = new Date(
        this.state.project.projectHistory[i].createdAt
      );
      bData.push({
        x:
          current_datetime.getFullYear() +
          "-" +
          (current_datetime.getMonth() + 1) +
          "-" +
          current_datetime.getDate() +
          " " +
          current_datetime.getHours() +
          ":" +
          current_datetime.getMinutes() +
          ":" +
          current_datetime.getSeconds(),
        y: bSum / this.state.project.projectHistory[i].fileDataScores.length,
      });
      aData.push({
        x:
          current_datetime.getFullYear() +
          "-" +
          (current_datetime.getMonth() + 1) +
          "-" +
          current_datetime.getDate() +
          " " +
          current_datetime.getHours() +
          ":" +
          current_datetime.getMinutes() +
          ":" +
          current_datetime.getSeconds(),
        y: aSum / this.state.project.projectHistory[i].fileDataScores.length,
      });

      var sData = [
        {
          label: "Accumulated Score",
          data: aData,
        },
        {
          label: "Base Score",
          data: bData,
        },
      ];
    }

    var axes = [
      { primary: true, type: "ordinal", position: "bottom" },
      { type: "linear", position: "left" },
    ];
    return (
      <div className="graphContainer">
        <div className="yLabel">Code Score</div>
        <div className="graphContent">
          <div className="xLabel">Project Code Score History Graph</div>
          <Chart style={{ height: "300px" }} data={sData} axes={axes} />
          <div className="xLabel">Date of commit</div>
        </div>
      </div>
    );
  };

  renderSearchGraph = () => {
    if (this.state.searchWord.length === 0) {
      return <div></div>;
    }

    var fileHistoryList = [];
    // Create list of fileHistories
    for (var i = 0; i < this.state.project.projectHistory.length; i++) {
      for (
        var j = 0;
        j < this.state.project.projectHistory[i].fileDataScores.length;
        j++
      ) {
        if (
          this.state.project.projectHistory[i].fileDataScores[j].fileName ===
          this.state.searchWord
        ) {
          fileHistoryList.push({
            date: this.state.project.projectHistory[i].createdAt,
            data: this.state.project.projectHistory[i].fileDataScores[j],
          });
        }
      }
    }

    if (fileHistoryList.length === 0) {
      return <div>Couldn't find file</div>;
    } else {
      if (fileHistoryList.length === 1) {
        return <div>Can't make graph of only one entry</div>;
      } else {
        var sData = [];
        for (var i = 0; i < fileHistoryList.length; i++) {
          var current_datetime = new Date(fileHistoryList[i].date);
          sData.push({
            x:
              current_datetime.getFullYear() +
              "-" +
              (current_datetime.getMonth() + 1) +
              "-" +
              current_datetime.getDate() +
              " " +
              current_datetime.getHours() +
              ":" +
              current_datetime.getMinutes() +
              ":" +
              current_datetime.getSeconds(),
            y: fileHistoryList[i].data.baseScore,
          });
        }
        var searchData = [
          {
            label: "Series 1",
            data: sData,
          },
        ];
        var searchAxes = [
          { primary: true, type: "ordinal", position: "bottom" },
          { type: "linear", position: "left" },
        ];
        return (
          <div className="graphContainer">
            <div className="yLabel">Code Score</div>
            <div className="graphContent">
              <div className="xLabel">
                {this.state.searchWord} - Code Score History{" "}
              </div>
              <Chart
                style={{ height: "300px" }}
                data={searchData}
                axes={searchAxes}
              />
              <div className="xLabel">Date of commit</div>
            </div>
          </div>
        );
      }
    }
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
            <div>{this.renderMainGraph()}</div>
            <div className="word">
              Search for file to get file score history
            </div>
            <input
              value={this.state.searchWord}
              onChange={this.onInputChange}
            ></input>
            {this.renderSearchGraph()}
            <div className="projectsSeperator"></div>
            <div>Project Commit History</div>
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
