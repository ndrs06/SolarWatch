import './SignIn.scss';
import React from 'react';
import { useNavigate } from "react-router-dom";
import { useProfile } from '../../contexts/ProfileContext';

import SignInForm from "../../components/SignInForm/SignInForm.jsx";

const postSignIn = (user) => {
    return fetch('/api/Auth/Login', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(user)
    }).then(resp => {
        if (!resp.ok) {
            throw new Error(`HTTP error! status: ${resp.status}`);
        }
        return resp.json()
    }).catch(err => {
        console.error('Error:', err);
    });
}

export default function SignIn() {
    const navigate = useNavigate();
    const { signIn } = useProfile();
    
    const handleSignIn = (user) => {
        postSignIn(user).then(data => {
            signIn();
            navigate("/solar-watch") 
            console.log(data);
        })
    }
    
    const props = {
        onSave: handleSignIn,
        onCancel: _ => navigate("/")
    }
    
    return (
        <>
            <SignInForm {...props}/>
        </>
    );
}