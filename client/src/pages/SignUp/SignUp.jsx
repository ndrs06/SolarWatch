import './SignUp.scss';
import React from 'react';
import {useNavigate} from "react-router-dom";

import Navbar from "../../components/Navbar/Navbar.jsx";
import SignUpForm from "../../components/SignUpForm/SignUpForm.jsx"

const postSignUp = (user) => {
    return fetch('/api/Auth/Registration', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(user)
    }).then(resp => {
        if (!resp.ok) {
            throw new Error(`HTTP error! status: ${resp.status}`);
        }
        return resp.ok
    }).catch(err => {
        console.error('Error:', err);
    });
}

export default function SignUp() {
    const navigate = useNavigate();

    const handleSignIn = (user) => {
        postSignUp(user).then( resp => resp ? navigate("/solar-watch") : navigate("/sign-up"))
    }

    const props = {
        onSave: handleSignIn,
        onCancel: _ => navigate("/")
    }
    return (
        <>
            <SignUpForm {...props}/>
        </>
    );
}