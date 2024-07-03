import './SolarWatch.scss';
import React, {useEffect, useState} from 'react';
import {useNavigate} from "react-router-dom";

import Navbar from "../../components/Navbar/Navbar.jsx";
import SolarWatchResult from "../../components/SolarWatchResult/SolarWatchResult.jsx"
import SolarWatchReqForm from "../../components/SolarWatchReqForm/SolarWatchReqForm.jsx";

const getSolarWatch = (cityName, date) => {
    return fetch(`/api/SolarWatch/?cityName=${cityName}&date=${date}`)
        .then(res => {
        if (!res.ok) {
            throw new Error(`HTTP error! status: ${res.status}`);
        }
            return res.json();
    })
        .catch(err => {
        console.error('Error:', err);
    });
}

export default function SolarWatch() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [authorised, setAuthorised] = useState(false)
    const [solarWatch, setSolarWatch] = useState(null);

    useEffect(() => {
        getSolarWatch()
            .then(solarWatch => setSolarWatch(solarWatch))
    }, []);

    const props = {
        solarWatch
    }

    return (
        <>
            <h1>SOLAR-WATCH</h1>
            <Navbar/>
            <SolarWatchReqForm {...props}/>
            <SolarWatchResult {...props}/>
        </>
    );
}