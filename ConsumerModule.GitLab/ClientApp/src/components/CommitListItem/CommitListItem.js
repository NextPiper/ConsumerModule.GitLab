import React from "react";

import style from "./commitListItem.css";

const CommitListItem = (props) => {
  return (
    <div onClick={() => props.onClick(props.id)} className="commitListItem">
      <div>
        <div>CommitSha: {props.sha.substring(0, 7)}</div>
        <div>UserId: {props.userId}</div>
      </div>
      <div>
        <div>
          avgCodeScore:{" "}
          {Math.round((props.avgCodeScore + Number.EPSILON) * 100) / 100}
        </div>
        <div>numberOfFiles: {props.numberOfFiles}</div>
      </div>
    </div>
  );
};

export default CommitListItem;
