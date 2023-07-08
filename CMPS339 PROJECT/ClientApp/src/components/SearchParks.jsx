import React, { useEffect, useState } from "react";
import axios from "axios";
import "./SearchParks.css";

function SearchParks() {
    const [searchTerm, setSearchTerm] = useState('');
    const [parks, setParks] = useState([]);

    useEffect(() => {
        axios
          .get("https://localhost:7056/api/amusement-parks")
          .then((res) => {
            setParks(res.data);
            console.log(res.data);
          })
          .catch((err) => console.log(err));
      }, []);

      const handleSearch = (event) => {
        setSearchTerm(event.target.value);
      };

      const filteredParks = parks.filter((park) => {
        if (searchTerm === '') {
            return false;
        }
        return park.name.toLowerCase().startsWith(searchTerm.toLowerCase());
    });
    
  return (
    <div>
      <input
        type="text"
        placeholder="Search Parks..."
        value={searchTerm}
        onChange={handleSearch}
      />
      <ul>
        {filteredParks.map((park) => (
          <li key={park.id}>{park.name}</li>
        ))}
      </ul>
    </div>
  );
};

export default SearchParks;