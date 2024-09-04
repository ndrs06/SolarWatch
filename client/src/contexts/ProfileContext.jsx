import React, {createContext, useContext, useState} from "react";

const ProfileContext = createContext(null);

export const useProfile = () => {
    return useContext(ProfileContext);
};

export const ProfileContextProvider = ({ children }) => {
    const signIn = () => {
        localStorage.setItem('isSignedIn', true);
        const logoutTime = new Date();
        logoutTime.setMinutes(logoutTime.getMinutes() + 30);
        localStorage.setItem('logoutTime', logoutTime.getTime());
    }
        
    const logout = () => {
        localStorage.removeItem('isSignedIn');
        localStorage.removeItem('logoutTime' );
    }

    return (
        <ProfileContext.Provider value={{ signIn, logout }}>
            { children }
        </ProfileContext.Provider>
    );
};