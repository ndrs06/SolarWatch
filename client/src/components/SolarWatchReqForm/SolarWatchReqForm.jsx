import './SolarWatchReqForm.scss';
import React, {useState} from "react";
import {Link} from "react-router-dom";

export default function SolarWatchReqForm(props) {
    const {
        solarWatch,
        onSave,
        onCancel
    } = props;

    const [cityName, setCityName] = useState("");
    const [date, setDate] = useState(null);

    const onSubmit = e => {
        e.preventDefault();
        return onSave({...solarWatch, cityName: cityName, date: date});
    }

    return (
        <form onSubmit={onSubmit}>
            <div>
                <label htmlFor="solar-watch-city" id="form1" type="text"/>
                <input
                    value={user.email}
                    onChange={e => setCityName(e.target.value)}
                    placeholder="Email"
                    type="text"
                    name="cityName"
                    id="solar-watch-city"
                />
            </div>
            <div>
                <label htmlFor="solar-watch-date" id="form1" type="date"/>
                <input
                    value={user.password}
                    onChange={e => setDate(e.target.value)}
                    placeholder="Password"
                    type="date"
                    name="date"
                    id="solar-watch-date"
                />
            </div>
            <div>
                <Link to="/sign-up">
                    <p>SignUp</p>
                </Link>
            </div>

            <button type="submit">SignIn</button>
            <button type="button" onClick={onCancel}>Cancel</button>

        </form>
    );

}