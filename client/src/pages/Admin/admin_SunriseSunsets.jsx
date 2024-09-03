import React, {useEffect, useState} from "react";
import Loading from "../../components/Loading/Loading.jsx"
import {Link, useParams} from "react-router-dom";
import loading from "../../components/Loading/Loading.jsx";

const getCity = (cityName) => fetch(`/api/admin/cities/${cityName}`)
    .then(res => res.json())
    .catch(err => console.log(err));

const postSunriseSunset = (cityName, date) => fetch(
    `/api/admin/sunrise-sunsets?cityName=${cityName}&date=${date}`, {
        method: "POST"
    }).then(res => res.json())
    .catch(err => console.log(err));

const deleteSunriseSunset = (cityName, date) => fetch(
    `/api/admin/sunrise-sunsets?cityName=${cityName}&date=${date}`, {
        method: "DELETE"
    }).catch(err => console.log(err));

export default function AdminSunriseSunsets() {
    const [loading, setLoading] = useState(true);
    const [plusSwitch, setPlusSwitch] = useState(false);
    const [date, setDate] = useState(null);
    const { cityName } = useParams();
    const [city, setCity] = useState(null);
    
    useEffect(_ => {
        getCity(cityName).then(data => {
            setLoading(false);
            setCity(data); 
        })
    }, [cityName]);

    const handlePost = (cityName, date) => {
        postSunriseSunset(cityName, date)
            .then(_ => getCity(cityName))
            .then(data => setCity(data))
    }
    
    const handleDelete = (cityName, date) => {
        deleteSunriseSunset(cityName, date)
            .then(_ => getCity(cityName))
            .then(data => setCity(data));
    }
    
    return (
        loading ? (
            <Loading/> 
        ) : (
            <table className="table">
                <thead>
                <tr>
                    <th>{city.name}</th>
                    <th>Date</th>
                    <th>Sunrise</th>
                    <th>Sunset</th>
                    {!plusSwitch ? (
                        <>
                            <th></th>
                            <th><button onClick={() => setPlusSwitch(!plusSwitch)}>+</button></th>
                        </>
                    ) : (
                        <>
                            <th>
                                <input type="date" onChange={e => setDate(e.target.value)}/>
                            </th>
                            <th>
                                <button onClick={() => {
                                    handlePost(cityName, date)
                                    setPlusSwitch(!plusSwitch)}}>Add</button>
                            </th>
                        </>
                    )}
                </tr>
                </thead>
                <tbody>
                {city.sunriseSunsets.map(sunriseSunset => (
                        <tr key={sunriseSunset.date}>
                            <td>{sunriseSunset.id}</td>
                            <td>{sunriseSunset.date.substring(0,10)}</td>
                            <td>{sunriseSunset.sunrise}</td>
                            <td>{sunriseSunset.sunset}</td>
                            <td>
                                <button>edit</button>
                            </td>
                            <td>
                                <button onClick={() => handleDelete(city.name, sunriseSunset.date)}>delete</button>
                            </td>
                        </tr>
                ))}
                </tbody>
            </table>
        )
    )
}