import React, { Component } from 'react';
import FetchParks from './FetchParks';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>
          <input type="text" name="TextBox" id="TextField" placeholder='Search Parks Here!'/>
        </h1>
        this da home page
        <h2>
          <FetchParks/>
          </h2>
        
      </div>
    );
  }
}
