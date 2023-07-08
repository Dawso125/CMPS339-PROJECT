import React, {Component} from "react";
import FetchParks from './FetchParks';

export class ParksListing extends Component {
    static displayName = ParksListing.name;

    render() {
        return (
          <div>
            <h1>
              <FetchParks/>
              </h1>
            
          </div>
        );
      }
}  