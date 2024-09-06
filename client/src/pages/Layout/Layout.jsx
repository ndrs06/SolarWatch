import "./Layout.scss"
import React, {useEffect} from "react";
import Navbar from "../../components/Navbar/Navbar.jsx";
import Footer from "../../components/Footer/Footer.jsx";
import {Outlet} from "react-router-dom";

export default function Layout() {
    
    return (
        <div className="layout">
            <div className="layout-header-container">
                <div className="layout-header"></div>
                <Navbar className="layout-navbar"/>
            </div>
            <Outlet className="layout-outlet"/>
            <Footer className="layout-footer"/>
        </div>
    );
}
