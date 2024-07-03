import './SignIn.scss';
import React from 'react';
import {useNavigate} from "react-router-dom";
import { useProfile } from '../../contexts/ProfileContext';

import Navbar from "../../components/Navbar/Navbar.jsx";
import SignInForm from "../../components/SignInForm/SignInForm.jsx";

const postSignIn = (user) => {
    return fetch('/api/Auth/Login', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(user)
    }).then(res => {
        if (!res.ok) {
            throw new Error(`HTTP error! status: ${res.status}`);
        }
        return res.ok
    }).catch(err => {
        console.error('Error:', err);
    });
}


export default function SignIn() {
    const navigate = useNavigate();
    const { login } = useProfile();
    
    const handleSignIn = (user) => {
        postSignIn(user).then( res => {
            if (!res)
            {
                navigate("/sign-in")                
            }
            login();
            navigate("/solar-watch")            
        })
    }
    
    const props = {
        onSave: handleSignIn,
        onCancel: _ => navigate("/")
    }
    
    return (
        <>
            <Navbar/>
            <SignInForm {...props}/>
        </>
    );
}