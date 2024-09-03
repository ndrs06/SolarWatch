import React, {useEffect, useState} from "react";
import Loading from "../../components/Loading/Loading.jsx"
import {Link, useNavigate} from "react-router-dom";

const getCities = () => fetch(
    "/api/admin/cities")
    .then(res => res.json())
    .catch(err => console.log(err));

const postCity = (cityName) => fetch(
    `/api/admin/cities?cityName=${cityName}`, {
        method: "POST"
    }).then(res => res.json())
    .catch(err => console.log(err));

const deleteCity = (cityName) => fetch(
    `/api/admin/cities?cityName=${cityName}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }       
    })
    .then(res => res.json())
    .catch(err => console.log(err));

export default function AdminCities() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [cities, setCities] = useState(null);
    const [editCity, setEditCity] = useState({});
    const [cityName, setCityName] = useState("");
    const [plusSwitch, setPlusSwitch] = useState(false);

    useEffect(() => {
        getCities()
            .then(data => {
            setLoading(false);
            setEditCity(data.map(city => editCity[city.name] = false));
            setCities(data);
            })
    },[]);
    
    const handleDelete = cityName => {
        deleteCity(cityName);
        setCities(prev => prev.filter(city => city.name !== cityName));
    }
    
    const handleEdit = cityName => {
        setEditCity({...editCity, [cityName]: !editCity[cityName]});
    }
    
    const handlePost = cityName => {
        postCity(cityName);
        getCities().then(data => setCities(data))
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
                    <th><button onClick={() => setPlusSwitch(!plusSwitch)}>+</button></th>
                </tr>
                {plusSwitch && 
                <tr>
                    <th><input onChange={e => setCityName(e.target.value)}></input></th>
                    <th><button onClick={() => handlePost(cityName)}>Add</button></th>
                </tr>
                }
                </thead>
                
                <tbody>
                {cities.map(city => (
                    !editCity[city.name] ? (
                        <tr key={city.name}>
                            <td>{city.name}</td>
                            <td>{city.state}</td>
                            <td>{city.country}</td>
                            <td>{city.lat}</td>
                            <td>{city.lon}</td>
                            <td>
                                <button onClick={() => navigate(`/admin/cities/${city.name}`)}>details</button>
                            </td>
                            <td>
                                <button onClick={() => handleEdit(city.name)}>edit</button>
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
                                <button onClick={() => navigate(`/admin/cities/${city.name}`)}>details</button>
                            </td>
                            <td>
                            <button onClick={() => handleEdit(city.name)}>edit</button>
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
