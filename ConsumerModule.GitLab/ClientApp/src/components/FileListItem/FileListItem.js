import React from "react";

import style from "./fileListItem.css";
import style2 from "../projects.css";

const FileListItem = (props) => {
  const [drawerOpen, setDrawerOpen] = React.useState(false);
  const onClick = () => {
    console.log("clicked");
  };

  const dropDownClicked = () => {
    console.log(drawerOpen);

    setDrawerOpen(!drawerOpen);
    console.log(drawerOpen);
  };

  const renderDropDown = () => {
    if (drawerOpen) {
      return <div className="moreDetailsContainer">{renderDictDetails()}</div>;
    } else {
      return <div></div>;
    }
  };

  const renderDictDetails = () => {
    var dict = props.detailedScoreDict;

    var list = [];

    for (var key in dict) {
      list.push(
        <div style={{ width: "100%" }}>
          {key}:{Math.round((dict[key] + Number.EPSILON) * 100) / 100}
          <hr></hr>
        </div>
      );
    }
    return list;
  };

  const renderDetails = () => {
    if (drawerOpen) {
      return <div>Show less...</div>;
    } else {
      return <div>Show more...</div>;
    }
  };

  return (
    <div className="fileListContainer">
      <div onClick={() => onClick()} className="fileListItem">
        <div>
          <div>Filename: {props.fileName}</div>
          <div>ref: master</div>
        </div>
        <div>
          <div>
            baseScore:{" "}
            {Math.round((props.baseScore + Number.EPSILON) * 100) / 100}
          </div>
          <div>
            accumulatedScore:{" "}
            {Math.round((props.accumulatedCodeScore + Number.EPSILON) * 100) /
              100}
          </div>
        </div>
      </div>
      <div className="pseperator"></div>
      <div className="moreDetailsList">{renderDropDown()}</div>
      <div onClick={() => dropDownClicked()} className="moreDetails">
        {renderDetails()}
      </div>
    </div>
  );
};

export default FileListItem;

/*

key={i}
        fileName={item.fileName}
        ref={item.ref}
        baseScore={item.baseScore}
        accumulatedCodeScore={item.accumulatedCodeScore}
        detailedScoreDict={item.detailedScoreDict}



*/
