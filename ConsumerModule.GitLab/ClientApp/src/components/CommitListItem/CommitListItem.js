import React from "react";

import style from "./commitListItem.css";

const CommitListItem = (props) => {
  console.log(props.fileName);

  return (
    <div onClick={() => props.onClick(props.id)} className="commitListItem">
      <div>
        <div>Filename: {props.fileName}</div>
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

/*

key={i}
        fileName={item.fileName}
        ref={item.ref}
        baseScore={item.baseScore}
        accumulatedCodeScore={item.accumulatedCodeScore}
        detailedScoreDict={item.detailedScoreDict}




*/
