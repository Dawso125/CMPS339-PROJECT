import React, { useEffect, useState } from "react";
import axios from "axios";
import { useTable } from "react-table";

function FetchParks() {
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

  const columns = React.useMemo(
    () => [
      {
        Header: "Name",
        accessor: "name",
      },
    ],
    []
  );

  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } =
    useTable({ columns, data: parks });
//this is way much more fancy ong
  return (
    <div>
      <table {...getTableProps()} style={{ borderCollapse: "collapse" }}>
        <thead>
          {headerGroups.map((headerGroup) => (
            <tr {...headerGroup.getHeaderGroupProps()}>
              {headerGroup.headers.map((column) => (
                <th
                  {...column.getHeaderProps()}
                  style={{
                    borderBottom: "1px solid black",
                    background: "aliceblue",
                    padding: "8px",
                  }}
                >
                  {column.render("Header")}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody {...getTableBodyProps()}>
          {rows.map((row) => {
            prepareRow(row);
            return (
              <tr {...row.getRowProps()}>
                {row.cells.map((cell) => (
                  <td
                    {...cell.getCellProps()}
                    style={{
                      borderBottom: "1px solid black",
                      padding: "8px",
                      textAlign: "left",
                    }}
                  >
                    {cell.render("Cell")}
                  </td>
                ))}
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}

export default FetchParks;

//     useEffect(() => {
//         axios.get("https://localhost:7056/api/amusement-parks")
//             .then(res => {
//                 console.log(res.data);
//                 setParks(res.data);
//             })
//             .catch(err => console.log(err));
//     }, []);

//     return (
//         <div>
//             {parks.map(park => (
//                 <div key={park.id}>
//                     <h2>{park.name}</h2>
//                 </div>
//             ))}
//         </div>
//     );
// }

// export default FetchParks;
