import './SignUpForm.scss';
import {Link} from "react-router-dom";
import React, {useState} from "react";

export default function SignInForm(props) {
    const {
        onSave,
        onCancel
    } = props;

    const [user, setUser] = useState({
        email: "",
        username: "",
        password: ""
    });

    const onSubmit = e => {
        e.preventDefault();
        return onSave(user);
    }

    return (
        <form onSubmit={onSubmit}>
            <div>
                <label htmlFor="email-sign-up" id="form1" type="email"/>
                <input
                    value={user.email}
                    onChange={e => setUser({...user, email: e.target.value})}
                    placeholder="Email"
                    type="email"
                    name="email"
                    id="email-sign-up"
                />
            </div>
            <div>
                <label htmlFor="username-sign-up" id="form1" type="text"/>
                <input
                    value={user.username}
                    onChange={e => setUser({...user, username: e.target.value})}
                    placeholder="Username"
                    type="text"
                    name="username"
                    id="username-sign-up"
                />
            </div>
            <div>
                <label htmlFor="password-sign-up" id="form1" type="password"/>
                <input
                    value={user.password}
                    onChange={e => setUser({...user, password: e.target.value})}
                    placeholder="Password"
                    type="password"
                    name="password"
                    id="password-sign-up"
                />
            </div>
            <div>
                <Link to="/sign-in">
                    <p>SignIn</p>
                </Link>
            </div>

            <button type="submit">SignUp</button>
            <button type="button" onClick={onCancel}>Cancel</button>

        </form>
    );

}