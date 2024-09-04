import React, {useEffect} from "react";
import Navbar from "../../components/Navbar/Navbar.jsx";
import Footer from "../../components/Footer/Footer.jsx";
import {Outlet} from "react-router-dom";

export default function Layout() {
    
    return (
        <div className="layout">
            <Navbar />
            <Outlet />
            <Footer />
        </div>
    );
}