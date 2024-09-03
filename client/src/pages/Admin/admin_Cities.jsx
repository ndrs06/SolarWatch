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

const updateCity = (city) => fetch(
    `/api/admin/cities/${city.name}`, {
        method: "PATCH",
        headers: {
            "accept": "text/plain",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(city)});

const deleteCity = (cityName) => fetch(
    `/api/admin/cities?cityName=${cityName}`, {
        method: "DELETE",      
    })
    .catch(err => console.log(err));

export default function AdminCities() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [cities, setCities] = useState(null);
    const [editCity, setEditCity] = useState({});
    const [cityName, setCityName] = useState("");
    const [plusSwitch, setPlusSwitch] = useState(false);
    const [cityState, setCity] = useState({
        name: "",
        state: "",
        country: "",
        lat: "",
        lon: "",
    })

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
    
    const handleEdit = (city) => {
        updateCity(city)
            .then(_ => getCities())
            .then(data => setCities(data));        
    }
    
    const handlePost = cityName => {
        postCity(cityName)
            .then(_ => getCities())
            .then(data => setCities(data))
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
                        {!plusSwitch ? (
                            <>
                                <th colSpan="2"></th>                    
                                <th><button onClick={() => setPlusSwitch(!plusSwitch)}>+</button></th>                        
                            </>
                        ) : (
                            <>
                                <th colSpan="2">
                                    <input placeholder="Name of the city" onChange={e => setCityName(e.target.value)}/>
                                </th>
                                <th>
                                    <button onClick={() => {
                                    handlePost(cityName)
                                    setPlusSwitch(!plusSwitch)}}>Add</button>
                                </th>
                            </>
                        )}
                    </tr>
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
                                <button onClick={() => {
                                    setEditCity({...editCity, [city.name]: !editCity[city.name]})
                                    setCity({...city})
                                }}>edit</button>
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
                                    value={cityState.state}
                                    onChange={e => setCity({...cityState, state: e.target.value})}
                                    placeholder="State"
                                    type="text"
                                    name="state"
                                    id="city-state"
                                />
                            </td>
                            <td>
                                <input
                                    value={cityState.country}
                                    onChange={e => setCity({...cityState, country: e.target.value})}
                                    placeholder="Country"
                                    type="text"
                                    name="country"
                                    id="city-country"
                                />
                            </td>
                            <td>
                                <input
                                    value={cityState.lat}
                                    onChange={e => setCity({...cityState, lat: e.target.value})}
                                    placeholder="Lat"
                                    type="text"
                                    name="lat"
                                    id="city-lat"
                                />
                            </td>
                            <td>
                                <input
                                    value={cityState.lon}
                                    onChange={e => setCity({...cityState, lon: e.target.value})}
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
                            <button type="submit"
                                onClick={() => {
                                handleEdit(cityState);
                                setEditCity({...editCity, [city.name]: !editCity[city.name]})
                            }}>submit</button>
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
