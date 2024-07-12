import './SolarWatch.scss';
import React, {useState} from 'react';

import SolarWatchResult from "../../components/SolarWatchResult/SolarWatchResult.jsx"
import SolarWatchReqForm from "../../components/SolarWatchReqForm/SolarWatchReqForm.jsx";

const getSolarWatch = (reqSolarWatch) => {
    return fetch(`/api/SolarWatch/?cityName=${reqSolarWatch.cityName}&date=${reqSolarWatch.date}`)
        .then(resp => {
        if (!resp.ok) {
            throw new Error(`HTTP error! status: ${resp.status}`);
        }
            return resp.json();
    })
        .catch(err => {
        console.error('Error:', err);
    });
}

export default function SolarWatch() {
    const [loading, setLoading] = useState(false);
    const [authorised, setAuthorised] = useState(false)
    const [solarWatch, setSolarWatch] = useState(null);
    
    const handleSolarWatch = reqSolarWatch => {
        getSolarWatch(reqSolarWatch).then(data => setSolarWatch(data))
    }

    const props = {
        solarWatch,
        setSolarWatch,
        onSave: handleSolarWatch
    }

    return (
        <>
            <h1>SOLAR-WATCH</h1>
            <SolarWatchReqForm {...props}/>
            { solarWatch != null ? <SolarWatchResult {...props}/> : <></> }
        </>
    );
}