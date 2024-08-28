import React, {useEffect, useState} from "react";
import Loading from "../../components/Loading/Loading.jsx"
import {Link, useParams} from "react-router-dom";
import loading from "../../components/Loading/Loading.jsx";

const getCity = (cityName) => fetch(`/api/admin/cities/${cityName}`)
    .then(res => res.json())
    .catch(err => console.log(err));

export default function AdminSunriseSunsets() {
    const [loading, setLoading] = useState(true);
    const { cityName } = useParams();
    
    const [city, setCity] = useState(null);
/*    const [city, setCity] = useState({
        name: "",
        state: "",
        country: "",
        lat: "",
        lon: ""
    });*/
    
    useEffect(_ => {
        getCity(cityName).then(data => {
            setLoading(false);
            setCity(data); 
        })
    }, [cityName]);

    
    return (
        loading ? (
            <Loading/> 
        ) : (
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
                    <th>
                        <button>+</button>
                    </th>
                </tr>
                </thead>
                <tbody>
                {city.sunriseSunsets.map(city => (
                    !editCity.cityName ? (
                        <tr key={city.name}>
                            <td>{city.name}</td>
                            <td>{city.state}</td>
                            <td>{city.country}</td>
                            <td>{city.lat}</td>
                            <td>{city.lon}</td>
                            <td>
                                <button onClick={e => navigate(`/admin/cities/${city.name}`)}>details</button>
                            </td>
                            <td>
                                <button onClick={e => handleEdit(city.name)}>edit</button>
                            </td>
                            <td>
                                <button onClick={() => handleDelete(city.name)}>delete</button>
                            </td>
                        </tr>
                    ) : (
                        <tr key={city.name}>
                            <td>{city.name}</td>
                            <td>
                                <input
                                    value={city.country}
                                    onChange={e => ({...city, country: e.target.value})}
                                    placeholder="Country"
                                    type="text"
                                    name="country"
                                    id="city-country"
                                />
                            </td>
                            <td>
                                <input
                                    value={city.state}
                                    onChange={e => ({...city, state: e.target.value})}
                                    placeholder="State"
                                    type="text"
                                    name="state"
                                    id="city-state"
                                />
                            </td>
                            <td>
                                <input
                                    value={city.lat}
                                    onChange={e => ({...city, lat: e.target.value})}
                                    placeholder="Lat"
                                    type="text"
                                    name="lat"
                                    id="city-lat"
                                />
                            </td>
                            <td>
                                <input
                                    value={city.lon}
                                    onChange={e => ({...city, lon: e.target.value})}
                                    placeholder="Lon"
                                    type="text"
                                    name="lon"
                                    id="coty-lon"
                                />
                            </td>
                            <td>
                                <Link to="/admin/cities/:name">
                                    <button>details</button>
                                </Link>
                            </td>
                            <td>
                                <button onClick={e => handleEdit(city.name)}>edit</button>
                            </td>
                            <td>
                                <button onClick={() => handleDelete(city.name)}>delete</button>
                            </td>
                        </tr>
                    )
                ))}
                </tbody>
            </table>
        )
    )
}