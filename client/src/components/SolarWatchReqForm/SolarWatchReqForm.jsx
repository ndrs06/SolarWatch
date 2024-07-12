import './SolarWatchReqForm.scss';
import React, {useState} from "react";

export default function SolarWatchReqForm(props) {
    const {
        onSave,
    } = props;

    const [reqSolarWatch, setReqSolarWatch] = useState({
        cityName: "",
        date: Date.now()
    });

    const onSubmit = e => {
        e.preventDefault();
        return onSave(reqSolarWatch);
    }

    return (
        <form onSubmit={onSubmit}>
            <div>
                <label htmlFor="solar-watch-city" id="form1" type="text"/>
                <input
                    value={reqSolarWatch.cityName}
                    onChange={e => setReqSolarWatch({...reqSolarWatch, cityName: e.target.value})}
                    placeholder="city name"
                    type="text"
                    name="cityName"
                    id="solar-watch-city"
                />
            </div>
            <div>
                <label htmlFor="solar-watch-date" id="form1" type="date"/>
                <input
                    value={reqSolarWatch.date}
                    onChange={e => setReqSolarWatch({...reqSolarWatch, date: e.target.value})}
                    placeholder="date"
                    type="date"
                    name="date"
                    id="solar-watch-date"
                />
            </div>

            <button type="submit">Show</button>

        </form>
    );

}