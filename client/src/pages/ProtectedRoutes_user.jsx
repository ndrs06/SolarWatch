import React, { useEffect } from 'react';
import { Navigate, Outlet, useNavigate } from 'react-router-dom';
import { useProfile } from '../contexts/ProfileContext.jsx';

export default function ProtectedRoutes() {
    const { logout } = useProfile();
    const navigate = useNavigate();
    const isSignedIn = localStorage.getItem('isSignedIn');
    const logoutTime = localStorage.getItem('logoutTime');

    const checkLoginStatus = () => {
        const currentTime = new Date().getTime();
        if (!isSignedIn || (logoutTime && currentTime > parseInt(logoutTime))) {
            logout();
            navigate('/sign-in');
        }
    };

    useEffect(() => {
        checkLoginStatus();
    }, [isSignedIn, logoutTime, logout, navigate]);

    return isSignedIn ? <Outlet /> : <Navigate to="/sign-in"/>;
}