import React, {useEffect, useState} from "react";
import Loading from "../../components/Loading/Loading.jsx"
import {Link, useNavigate} from "react-router-dom";

const fetchCities = () => fetch(
    "/api/admin/cities")
    .then(res => res.json());

const deleteCity = (cityName) => fetch(
    `/api/admin/cities?cityName=${cityName}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }       
    })
    .then(res => res.json());


export default function AdminCities() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [cities, setCities] = useState(null);
    const [editCity, setEditCity] = useState({});



    useEffect(() => {
        fetchCities().then(data => {
            setLoading(false);            
            setCities(data)
            console.log(data)
            return data
        }).then(data => data.map(city => setEditCity({...editCity, [city.name]: false})));        
    },[]);
    
    const handleDelete = cityName => {
        deleteCity(cityName);
        setCities(prev => prev.filter(city => city.name !== cityName));
    }
    
    const handleEdit = cityName => {
        setEditCity({...editCity, [cityName]: !editCity[cityName]});
        console.log(editCity);
    }
    
    const props = {
        cities,
        setCities,
    };
    
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
                {cities.map(city => (
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