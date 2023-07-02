import React, {useEffect, useState} from "react";
import axios from 'axios';

function FetchParks() {
    const [parks, setParks] = useState([]);

    useEffect(  () => {
        axios.get("/api/amusement-parks")
        .then(res => setParks(res.data))
        .then(res => console.log(res.data))
        .catch(err => console.log(err))
     }, [])
    return (
        <div>{parks.map(park => (
            <div key={park.id}>
                <h2>{park.name}</h2>
                <p>{park.description}</p>
            </div>
        ))}
        </div>
    );
}

export default FetchParks