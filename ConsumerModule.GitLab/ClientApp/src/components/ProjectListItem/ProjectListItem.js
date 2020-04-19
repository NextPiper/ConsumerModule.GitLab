import React from "react";

import style from "./projectListItem.css";

const ProjectListItem = (props) => {
  return (
    <div
      onClick={() => props.onClick(props.projectId)}
      className="projectListItem"
    >
      <div>ProjectName: {props.projectName}</div>
      <div>
        Project Average Score:{" "}
        {Math.round((props.projectScore + Number.EPSILON) * 100) / 100}
      </div>
    </div>
  );
};

export default ProjectListItem;
