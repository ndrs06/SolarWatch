import "./Layout.scss"
import React from "react";
import Navbar from "../../components/Navbar/Navbar.jsx";
import Footer from "../../components/Footer/Footer.jsx";
import {Outlet} from "react-router-dom";

export default function Layout() {
    
    return (
        <div className="layout">
            <div className="layout-header-container">
                <div className="layout-header"><h1>Solar Watch</h1></div>
                <Navbar />
            </div>
            <div  className="layout-outlet">
                <Outlet />                
            </div>
            <Footer className="layout-footer"/>
        </div>
    );
}
