import './SolarWatchResult.scss';
import React from "react";

export default function SolarWatchResult(props) {
    const {
        solarWatch
    } = props
   
    return (
        <>
            <p>City: {solarWatch.city}</p>             
            <p>Date: {solarWatch.date.slice(0, 10)}</p>
            <p>Sunrise: {solarWatch.sunrise} - Sunset: {solarWatch.sunset}</p>
        </>
    )

}