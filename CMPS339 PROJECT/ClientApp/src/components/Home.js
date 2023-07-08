import React, { Component } from "react";
import SearchParks from "./SearchParks";
import "./Home.css"

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div className="Home-Container">
        <div className="Home-Center">
          <h1>Find Parks</h1>
          <div className="SearchParks-Container">
            <SearchParks />
          </div>
        </div>
      </div>
    );
  }
}
