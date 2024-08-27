import React, {useEffect, useState} from "react";
import Loading from "../Loading/Loading.jsx";

const fetchCities = () => fetch(
    "/api/admin/cities")
    .then(res => res.json());
export default function AdminCities() {
    const [loading, setLoading] = useState(true);
    const [cities, setCities] = useState(null);
    
    useEffect(() => {
        fetchCities().then(cities => {
            setLoading(false);
            setCities(cities)
        });        
    },[]);
    
    
    return (
        loading ? <Loading /> : (
            <table className="table">
                <thead>
                <tr>
                    <th>Name</th>
                    <th>State</th>
                    <th>Country</th>
                    <th>Lat.</th>
                    <th>Lon.</th>
                    <th></th>
                    <th></th>
                    <th><button>+</button></th>
                </tr>
                </thead>
                <tbody>
                {cities && cities.map(city => (
                    <tr key={city.name}>
                        <td>{city.name}</td>
                        <td>{city.state}</td>
                        <td>{city.country}</td>
                        <td>{city.lat}</td>
                        <td>{city.lon}</td>
                        <td><button>edit</button></td>
                        <td><button>delete</button></td>
                    </tr>
                ))}
                </tbody>
            </table>
        )
    )
}