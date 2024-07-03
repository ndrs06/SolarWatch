import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useProfile } from '../../contexts/ProfileContext.jsx'

export default function ProtectedRoute() {
    const { isAuthenticated } = useProfile();

    return isAuthenticated ? <Outlet /> : <Navigate to="/sign-in" />;
};